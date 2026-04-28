const textEncoder = new TextEncoder()
const textDecoder = new TextDecoder()
const DEFAULT_SECRET = 'artauction-frontend-beginner-secure-key-2026'
const IV_SIZE = 12

let cachedCryptoKeyPromise = null

function toBase64(binaryString) {
  if (typeof btoa === 'function') {
    return btoa(binaryString)
  }

  return Buffer.from(binaryString, 'binary').toString('base64')
}

function fromBase64(base64Value) {
  if (typeof atob === 'function') {
    return atob(base64Value)
  }

  return Buffer.from(base64Value, 'base64').toString('binary')
}

function bytesToBase64(bytes) {
  let binaryString = ''

  bytes.forEach((value) => {
    binaryString += String.fromCharCode(value)
  })

  return toBase64(binaryString)
}

function base64ToBytes(base64Value) {
  const binaryString = fromBase64(base64Value)
  const bytes = new Uint8Array(binaryString.length)

  for (let index = 0; index < binaryString.length; index += 1) {
    bytes[index] = binaryString.charCodeAt(index)
  }

  return bytes
}

function normalizeSecret(rawSecret) {
  const asString = String(rawSecret || DEFAULT_SECRET)

  if (asString.length >= 32) {
    return asString.slice(0, 32)
  }

  return asString.padEnd(32, '0')
}

async function getAesKey() {
  if (!globalThis.crypto?.subtle) {
    return null
  }

  if (!cachedCryptoKeyPromise) {
    const secret = normalizeSecret(import.meta.env.VITE_CLIENT_CRYPTO_KEY)
    cachedCryptoKeyPromise = globalThis.crypto.subtle.importKey(
      'raw',
      textEncoder.encode(secret),
      { name: 'AES-GCM' },
      false,
      ['encrypt', 'decrypt'],
    )
  }

  return cachedCryptoKeyPromise
}

function encodeFallback(value) {
  return `b64:${bytesToBase64(textEncoder.encode(value))}`
}

function decodeFallback(payload) {
  const normalizedPayload = String(payload || '')
  if (!normalizedPayload.startsWith('b64:')) {
    return normalizedPayload
  }

  const bytes = base64ToBytes(normalizedPayload.slice(4))
  return textDecoder.decode(bytes)
}

export async function encryptText(value) {
  const plainText = String(value ?? '')
  const aesKey = await getAesKey()

  if (!aesKey || !globalThis.crypto?.getRandomValues) {
    return encodeFallback(plainText)
  }

  const iv = globalThis.crypto.getRandomValues(new Uint8Array(IV_SIZE))
  const encryptedBuffer = await globalThis.crypto.subtle.encrypt(
    { name: 'AES-GCM', iv },
    aesKey,
    textEncoder.encode(plainText),
  )

  return `enc:${bytesToBase64(iv)}:${bytesToBase64(new Uint8Array(encryptedBuffer))}`
}

export async function decryptText(payload) {
  const normalizedPayload = String(payload ?? '')

  if (!normalizedPayload.startsWith('enc:')) {
    return decodeFallback(normalizedPayload)
  }

  if (!globalThis.crypto?.subtle) {
    return ''
  }

  const segments = normalizedPayload.split(':')
  if (segments.length !== 3) {
    return ''
  }

  try {
    const aesKey = await getAesKey()
    if (!aesKey) {
      return ''
    }

    const iv = base64ToBytes(segments[1])
    const encryptedBytes = base64ToBytes(segments[2])
    const decryptedBuffer = await globalThis.crypto.subtle.decrypt(
      { name: 'AES-GCM', iv },
      aesKey,
      encryptedBytes,
    )

    return textDecoder.decode(decryptedBuffer)
  } catch {
    return ''
  }
}

export async function createEncryptedFields(source, fields) {
  const encryptedFields = {}

  await Promise.all(
    fields.map(async (fieldName) => {
      encryptedFields[`${fieldName}Encrypted`] = await encryptText(source[fieldName] ?? '')
    }),
  )

  return encryptedFields
}

export async function readUserField(user, fieldName) {
  if (user?.[fieldName] !== undefined && user?.[fieldName] !== null) {
    return String(user[fieldName])
  }

  // const encryptedFieldName = `${fieldName}Encrypted`
  // if (!user?.[encryptedFieldName]) {
  //   return ''
  // }

  // return decryptText(user[encryptedFieldName])
}

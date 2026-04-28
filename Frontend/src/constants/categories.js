import { getAllCategories } from "../api/CategoryApis"

const test = await getAllCategories();
export const ARTWORK_CATEGORIES = test;
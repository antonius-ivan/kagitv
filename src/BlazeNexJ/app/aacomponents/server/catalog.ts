import "server-only";

import type {
  CatalogBrand,
  CatalogItem,
  CatalogItemType,
  CatalogQuery,
  CatalogResult
} from "@/app/types";
import { resolveServiceBaseUrl } from "./runtime";

const salesItemApiVersion = "2.0";

type CatalogApiBrand = {
  id: number;
  brand: string;
};

type CatalogApiType = {
  id: number;
  type: string;
};

type CatalogApiItem = {
  id: number;
  name: string;
  description?: string | null;
  basePrice?: number;
  price?: number;
  baseCurrencyCode?: string | null;
  currencyCode?: string | null;
  pictureUrl?: string | null;
  pictureUri?: string | null;
  salesItemBrandId?: number;
  catalogBrandId?: number;
  salesItemBrand?: CatalogApiBrand | null;
  catalogBrand?: CatalogApiBrand | null;
  salesItemTypeId?: number;
  catalogTypeId?: number;
  salesItemType?: CatalogApiType | null;
  catalogType?: CatalogApiType | null;
};

type CatalogApiResult = {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: CatalogApiItem[];
};

function getSalesItemApiBaseUrl() {
  const value = resolveServiceBaseUrl(
    [
      process.env.SALESITEM_API_URL,
      process.env["services__salesitem-api__http__0"],
      process.env["services__salesitem-api__https__0"],
      process.env["services__salesitem-api__http"],
      process.env["services__salesitem-api__https"],
      process.env["SERVICES__SALESITEM-API__HTTP__0"],
      process.env["SERVICES__SALESITEM-API__HTTPS__0"],
      process.env["SERVICES__SALESITEM-API__HTTP"],
      process.env["SERVICES__SALESITEM-API__HTTPS"],
      process.env["services__salesitem_api__http__0"],
      process.env["services__salesitem_api__https__0"],
      process.env["services__salesitem_api__http"],
      process.env["services__salesitem_api__https"],
      process.env["SERVICES__SALESITEM_API__HTTP__0"],
      process.env["SERVICES__SALESITEM_API__HTTPS__0"],
      process.env["SERVICES__SALESITEM_API__HTTP"],
      process.env["SERVICES__SALESITEM_API__HTTPS"]
    ],
    /(salesitem|salesitem-api|salesitem_api)/i
  );

  if (!value) {
    throw new Error("SalesItem API base URL is not configured.");
  }

  return value;
}

async function fetchCatalogApi<T>(path: string) {
  const url = new URL(`${getSalesItemApiBaseUrl()}${path}`);

  if (!url.searchParams.has("api-version")) {
    url.searchParams.set("api-version", salesItemApiVersion);
  }

  const response = await fetch(url, {
    cache: "no-store",
    headers: {
      Accept: "application/json"
    }
  });

  if (!response.ok) {
    throw new Error(`Catalog request failed: ${response.status} ${response.statusText}`);
  }

  return (await response.json()) as T;
}

function normalizeBrand(brand: CatalogApiBrand | null | undefined): CatalogBrand {
  return {
    id: brand?.id ?? 0,
    brand: brand?.brand ?? "Unknown"
  };
}

function normalizeType(itemType: CatalogApiType | null | undefined): CatalogItemType {
  return {
    id: itemType?.id ?? 0,
    type: itemType?.type ?? "Unknown"
  };
}

function pictureProxyUrl(id: number) {
  return `/api/catalog/items/${id}/pic`;
}

function normalizeItem(item: CatalogApiItem): CatalogItem {
  return {
    id: item.id,
    name: item.name,
    description: item.description ?? "No description available yet.",
    basePrice: item.basePrice ?? item.price ?? 0,
    baseCurrencyCode: item.baseCurrencyCode ?? item.currencyCode ?? "USD",
    pictureUrl: pictureProxyUrl(item.id),
    salesItemBrandId:
      item.salesItemBrandId ?? item.catalogBrandId ?? item.salesItemBrand?.id ?? item.catalogBrand?.id ?? 0,
    salesItemBrand: normalizeBrand(item.salesItemBrand ?? item.catalogBrand),
    salesItemTypeId:
      item.salesItemTypeId ?? item.catalogTypeId ?? item.salesItemType?.id ?? item.catalogType?.id ?? 0,
    salesItemType: normalizeType(item.salesItemType ?? item.catalogType)
  };
}

function buildCatalogItemsPath(query: CatalogQuery) {
  const searchParams = new URLSearchParams();
  searchParams.set("pageIndex", String(query.pageIndex ?? 0));
  searchParams.set("pageSize", String(query.pageSize ?? 12));

  if (query.brand) {
    searchParams.set("brand", String(query.brand));
  }

  if (query.type) {
    searchParams.set("type", String(query.type));
  }

  if (query.name) {
    searchParams.set("name", query.name);
  }

  return `/api/salesitem/items?${searchParams.toString()}`;
}

export async function getCatalogItems(query: CatalogQuery = {}): Promise<CatalogResult> {
  const payload = await fetchCatalogApi<CatalogApiResult>(buildCatalogItemsPath(query));

  return {
    pageIndex: payload.pageIndex,
    pageSize: payload.pageSize,
    count: payload.count,
    data: payload.data.map(normalizeItem)
  };
}

export async function getCatalogItem(id: number) {
  const payload = await fetchCatalogApi<CatalogApiItem>(`/api/salesitem/items/${id}`);
  return normalizeItem(payload);
}

export async function getCatalogItemsByIds(ids: number[]) {
  if (ids.length === 0) {
    return [] satisfies CatalogItem[];
  }

  const searchParams = new URLSearchParams();

  for (const id of ids) {
    searchParams.append("ids", String(id));
  }

  const payload = await fetchCatalogApi<CatalogApiItem[]>(`/api/salesitem/items/by?${searchParams.toString()}`);
  return payload.map(normalizeItem);
}

export async function getCatalogBrands() {
  const payload = await fetchCatalogApi<CatalogApiBrand[]>("/api/salesitem/salesitembrands");
  return payload.map((brand) => ({ id: brand.id, brand: brand.brand }));
}

export async function getCatalogTypes() {
  const payload = await fetchCatalogApi<CatalogApiType[]>("/api/salesitem/salesitemtypes");
  return payload.map((itemType) => ({ id: itemType.id, type: itemType.type }));
}

export async function getCatalogItemPicture(id: number) {
  const url = new URL(`${getSalesItemApiBaseUrl()}/api/salesitem/items/${id}/pic`);
  url.searchParams.set("api-version", salesItemApiVersion);

  const response = await fetch(url, {
    cache: "no-store"
  });

  if (!response.ok) {
    throw new Error(`Catalog image request failed: ${response.status}`);
  }

  return response;
}

export function formatCatalogPrice(amount: number, currencyCode: string) {
  const locale = currencyCode === "IDR" ? "id-ID" : "en-US";

  return new Intl.NumberFormat(locale, {
    style: "currency",
    currency: currencyCode,
    maximumFractionDigits: 2
  }).format(amount);
}
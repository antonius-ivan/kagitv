export type SalesitemDraft = { name: string; description: string };

export type Salesitem = SalesitemDraft & { slug: string };

export type CatalogBrand = {
  id: number;
  brand: string;
};

export type CatalogItemType = {
  id: number;
  type: string;
};

export type CatalogItem = {
  id: number;
  name: string;
  description: string;
  basePrice: number;
  baseCurrencyCode: string;
  pictureUrl: string;
  salesItemBrandId: number;
  salesItemBrand: CatalogBrand;
  salesItemTypeId: number;
  salesItemType: CatalogItemType;
};

export type MenuNode = {
  id: number;
  name: string;
  path?: string;
  icon?: string;
  children: MenuNode[];
};

export type MenuModule = {
  code: string;
  name: string;
  children: MenuNode[];
};

export type CatalogResult = {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: CatalogItem[];
};

export type CatalogQuery = {
  pageIndex?: number;
  pageSize?: number;
  brand?: number;
  type?: number;
  name?: string;
};

export type ContactRecord = {
  id: string;
  first?: string;
  last?: string;
  avatar?: string;
  twitter?: string;
  notes?: string;
  favorite?: boolean;
  createdAt: string;
};

export type WisataItem = {
  wisataId: number;
  nama: string;
  kota: string;
  harga: number;
  createdDate: string;
};

export type WisataListResponse = {
  page: number;
  pageSize: number;
  totalCount: number;
  items: WisataItem[];
};

export type WisataReportResponse = {
  items: WisataItem[];
  totalHarga: number;
  printedAt: string;
};

export type WisataUpsertInput = {
  nama: string;
  kota: string;
  harga: number;
};

export type WisataAuthPayload = {
  accessToken: string;
  accessTokenExpiresAt: string;
  username: string;
};
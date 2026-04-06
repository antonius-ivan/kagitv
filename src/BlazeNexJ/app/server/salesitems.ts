import type { CatalogItem, CatalogResult, Salesitem, SalesitemDraft } from "@/app/types";

function slugify(value: string) {
  return value
    .trim()
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/^-+|-+$/g, "") || "sales-item";
}

function createUniqueSlug(items: Salesitem[], name: string) {
  const baseSlug = slugify(name);
  let slug = baseSlug;
  let index = 2;

  while (items.some((item) => item.slug === slug)) {
    slug = `${baseSlug}-${index}`;
    index += 1;
  }

  return slug;
}

function normalizeSalesitems(items: CatalogItem[]): Salesitem[] {
  return items.map((item, index, entries) => {
    const priorItems = entries.slice(0, index).map((entry) => ({
      name: entry.name.trim(),
      description: entry.description.trim(),
      slug: slugify(entry.name)
    }));

    return {
      name: item.name.trim(),
      description: item.description.trim(),
      slug: createUniqueSlug(priorItems, item.name)
    };
  });
}

function findServiceDiscoveryEndpoint(protocol: "http" | "https") {
  const suffixPattern = new RegExp(`^services__salesitem-api__${protocol}__\\d+$`, "i");

  return Object.entries(process.env)
    .filter(([key, value]) => suffixPattern.test(key) && typeof value === "string" && value.length > 0)
    .map(([, value]) => value as string)
    .sort()[0];
}

function getSalesItemApiBaseUrl() {
  const serviceDiscoveryUrl = findServiceDiscoveryEndpoint("http") ?? findServiceDiscoveryEndpoint("https");

  if (serviceDiscoveryUrl) {
    return serviceDiscoveryUrl.replace(/\/+$/, "");
  }

  const configuredBaseUrl = process.env.BLAZENEXJ_SALESITEM_API_URL;

  if (!configuredBaseUrl) {
    throw new Error("Sales item API URL is not configured in BLAZENEXJ_SALESITEM_API_URL or Aspire service discovery.");
  }

  return configuredBaseUrl.replace(/\/+$/, "");
}

async function fetchSalesitemsPage(pageIndex: number, pageSize: number) {
  const response = await fetch(
    `${getSalesItemApiBaseUrl()}/api/salesitem/items?api-version=2.0&pageIndex=${pageIndex}&pageSize=${pageSize}`,
    {
      cache: "no-store"
    }
  );

  if (!response.ok) {
    throw new Error(`Sales item API returned ${response.status}.`);
  }

  return (await response.json()) as CatalogResult;
}

async function fetchAllCatalogItems() {
  const pageSize = 100;
  const items: CatalogItem[] = [];
  let pageIndex = 0;
  let totalCount = 0;

  do {
    const page = await fetchSalesitemsPage(pageIndex, pageSize);
    items.push(...page.data);
    totalCount = page.count;
    pageIndex += 1;
  } while (items.length < totalCount);

  return items;
}

export async function listSalesitems() {
  const items = await fetchAllCatalogItems();
  return normalizeSalesitems(items);
}

export async function getSalesitem(slug: string) {
  const items = await listSalesitems();
  return items.find((item) => item.slug === slug);
}

export function createSalesitem(input: SalesitemDraft) {
  const name = input.name.trim();
  const description = input.description.trim();

  if (!name || !description) {
    throw new Error("Name and description are required.");
  }

  throw new Error(`Live salesitem creation is not supported for ${name}.`);
}
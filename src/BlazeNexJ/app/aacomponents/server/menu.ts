import "server-only";

import type { MenuModule } from "@/app/types";
import { getSession } from "./auth";

function getWisataModule(): MenuModule {
  return {
    code: "WISATA",
    name: "Wisata",
    children: [
      { id: 201, name: "List Wisata", path: "/wisata", icon: "map", children: [] },
      { id: 202, name: "Tambah Wisata", path: "/wisata/tambah", icon: "add", children: [] },
      { id: 203, name: "Laporan", path: "/wisata/laporan", icon: "chart", children: [] }
    ]
  };
}

function hasWisataRoutes(modules: MenuModule[]) {
  return modules.some((module) =>
    module.code === "WISATA" ||
    module.children.some((node) => node.path?.startsWith("/wisata") || node.children.some((child) => child.path?.startsWith("/wisata")))
  );
}

function withWisataModule(modules: MenuModule[]) {
  return hasWisataRoutes(modules) ? modules : [...modules, getWisataModule()];
}

function getPreviewMenu(): MenuModule[] {
  return withWisataModule([
    {
      code: "SALES",
      name: "Sales",
      children: [
        { id: 1, name: "Dashboard", path: "/dashboard", icon: "board", children: [] },
        {
          id: 2,
          name: "Catalog",
          icon: "box",
          children: [
            { id: 3, name: "Products", path: "/salesitem", icon: "tag", children: [] },
            { id: 4, name: "Pricing", path: "/dashboard/catalog/pricing", icon: "money", children: [] },
            { id: 5, name: "Inventory", path: "/dashboard/catalog/inventory", icon: "cube", children: [] }
          ]
        },
        { id: 6, name: "Reports", path: "/dashboard/reports", icon: "chart", children: [] }
      ]
    }
  ]);
}

function findServiceDiscoveryEndpoint(protocol: "http" | "https") {
  const suffixPattern = new RegExp(`^services__salesitem-api__${protocol}__\\d+$`, "i");

  return Object.entries(process.env)
    .filter(([key, value]) => suffixPattern.test(key) && typeof value === "string" && value.length > 0)
    .map(([, value]) => value as string)
    .sort()[0];
}

function getSalesItemApiResolution() {
  const discoveredHttpUrl = findServiceDiscoveryEndpoint("http") ?? null;
  const discoveredHttpsUrl = findServiceDiscoveryEndpoint("https") ?? null;
  const configuredBaseUrl = process.env.BLAZENEXJ_SALESITEM_API_URL ?? null;
  const baseUrl = discoveredHttpUrl ?? discoveredHttpsUrl ?? configuredBaseUrl;

  return {
    baseUrl: baseUrl ? baseUrl.replace(/\/+$/, "") : null,
    configuredBaseUrl,
    discoveredHttpUrl,
    discoveredHttpsUrl
  };
}

function getSalesItemApiBaseUrl() {
  return getSalesItemApiResolution().baseUrl;
}

async function fetchSalesItemApi<T>(path: string) {
  const session = await getSession();
  const baseUrl = getSalesItemApiBaseUrl();

  if (!session?.accessToken || !baseUrl) {
    return getPreviewMenu() as T;
  }

  const response = await fetch(new URL(path, baseUrl), {
    cache: "no-store",
    headers: {
      Accept: "application/json",
      Authorization: `Bearer ${session.accessToken}`
    }
  });

  if (!response.ok) {
    const responseText = (await response.text()).trim();
    throw new Error(
      responseText
        ? `Menu request failed: ${response.status} ${response.statusText} - ${responseText}`
        : `Menu request failed: ${response.status} ${response.statusText}`
    );
  }

  return (await response.json()) as T;
}

export async function getRoleMenu() {
  try {
    return withWisataModule(await fetchSalesItemApi<MenuModule[]>("/api/menu"));
  } catch {
    return getPreviewMenu();
  }
}

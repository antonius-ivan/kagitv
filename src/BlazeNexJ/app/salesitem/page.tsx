import TwDefaultLayout from "../aacomponents/layouts/TwDefaultLayout";
import { TwSalesitemListLayout } from "../aacomponents/layouts/TwSalesitemListLayout";
import { getCatalogBrands, getCatalogItems, getCatalogTypes } from "../aacomponents/server/catalog";

export const dynamic = "force-dynamic";

type SalesitemPageProps = {
  searchParams: Promise<{
    brand?: string;
    type?: string;
    pageIndex?: string;
    name?: string;
  }>;
};

export default async function SalesitemPage({ searchParams }: SalesitemPageProps) {
  const resolvedSearchParams = await searchParams;
  const selectedBrandId = Number(resolvedSearchParams.brand);
  const selectedTypeId = Number(resolvedSearchParams.type);
  const pageIndex = Number(resolvedSearchParams.pageIndex);
  const nameFilter = resolvedSearchParams.name?.trim();

  try {
    const [catalog, brands, itemTypes] = await Promise.all([
      getCatalogItems({
        brand: Number.isFinite(selectedBrandId) ? selectedBrandId : undefined,
        type: Number.isFinite(selectedTypeId) ? selectedTypeId : undefined,
        name: nameFilter || undefined,
        pageIndex: Number.isFinite(pageIndex) ? pageIndex : 0,
        pageSize: 12
      }),
      getCatalogBrands(),
      getCatalogTypes()
    ]);

    return (
      <TwDefaultLayout
        eyebrow="Catalog"
        title="Browse the live salesitem catalog through the new storefront foundation."
        description="This route now reads the real SalesItem API contract instead of the temporary local-only bootstrap data."
        search={{
          action: "/salesitem",
          placeholder: "Search the live salesitem catalog",
          defaultValue: nameFilter,
          hiddenFields: [
            ...(Number.isFinite(selectedBrandId) ? [{ name: "brand", value: String(selectedBrandId) }] : []),
            ...(Number.isFinite(selectedTypeId) ? [{ name: "type", value: String(selectedTypeId) }] : [])
          ]
        }}
      >
        <TwSalesitemListLayout
          brands={brands}
          catalog={catalog}
          itemTypes={itemTypes}
          selectedBrandId={Number.isFinite(selectedBrandId) ? selectedBrandId : undefined}
          selectedTypeId={Number.isFinite(selectedTypeId) ? selectedTypeId : undefined}
        />
      </TwDefaultLayout>
    );
  } catch (error) {
    const message = error instanceof Error ? error.message : "Catalog is not available.";

    return (
      <TwDefaultLayout
        eyebrow="Catalog"
        title="Catalog service is not available yet."
        description="The storefront foundation is in place, but the live SalesItem API still needs to be reachable from BlazeNexJ."
        search={{
          action: "/salesitem",
          placeholder: "Search the live salesitem catalog",
          defaultValue: nameFilter
        }}
      >
        <section className="rounded-[1.6rem] border border-dashed border-stone-300 bg-stone-50 p-8 text-stone-600">
          {message}
        </section>
      </TwDefaultLayout>
    );
  }
}
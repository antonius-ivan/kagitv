import type { CatalogBrand, CatalogItemType, CatalogResult } from "@/app/types";
import Link from "next/link";

type TwSalesitemListLayoutProps = {
  catalog: CatalogResult;
  brands: CatalogBrand[];
  itemTypes: CatalogItemType[];
  selectedBrandId?: number;
  selectedTypeId?: number;
};

function buildCatalogHref(brandId?: number, typeId?: number) {
  const searchParams = new URLSearchParams();

  if (brandId) {
    searchParams.set("brand", String(brandId));
  }

  if (typeId) {
    searchParams.set("type", String(typeId));
  }

  const queryString = searchParams.toString();
  return queryString ? `/salesitem?${queryString}` : "/salesitem";
}

function buildCatalogSummary(description: string, maxLength = 144) {
  if (description.length <= maxLength) {
    return description;
  }

  const truncatedDescription = description.slice(0, maxLength);
  const lastWhitespaceIndex = truncatedDescription.lastIndexOf(" ");

  if (lastWhitespaceIndex <= 0) {
    return `${truncatedDescription.trimEnd()}...`;
  }

  return `${truncatedDescription.slice(0, lastWhitespaceIndex).trimEnd()}...`;
}

function formatPrice(amount: number, currencyCode: string) {
  return new Intl.NumberFormat(currencyCode === "IDR" ? "id-ID" : "en-US", {
    style: "currency",
    currency: currencyCode,
    maximumFractionDigits: 2
  }).format(amount);
}

export function TwSalesitemListLayout({
  catalog,
  brands,
  itemTypes,
  selectedBrandId,
  selectedTypeId
}: TwSalesitemListLayoutProps) {
  const selectedBrand = brands.find((brand) => brand.id === selectedBrandId);
  const selectedItemType = itemTypes.find((itemType) => itemType.id === selectedTypeId);
  const activeFilterCount = Number(Boolean(selectedBrandId)) + Number(Boolean(selectedTypeId));

  return (
    <section className="grid gap-6 lg:grid-cols-[19rem_minmax(0,1fr)] xl:gap-8" id="content">
      <aside className="h-fit rounded-[1.9rem] border border-stone-200 bg-white p-6 shadow-[0_14px_32px_rgba(90,67,42,0.08)] lg:sticky lg:top-6">
        <div className="border-b border-stone-200 pb-5">
          <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">Refine Catalog</p>
          <h3 className="mt-3 text-2xl font-semibold tracking-tight text-stone-950">Shop by fit</h3>
          <p className="mt-3 text-sm leading-7 text-stone-600">
            Narrow the storefront before you scan the live sales item grid.
          </p>
        </div>

        <div className="mt-5 flex flex-wrap gap-2">
          <span className="rounded-full border border-orange-200 bg-orange-50 px-3 py-1 text-xs font-semibold uppercase tracking-[0.22em] text-orange-700">
            {catalog.count} items
          </span>
          <span className="rounded-full border border-stone-200 px-3 py-1 text-xs font-semibold uppercase tracking-[0.22em] text-stone-600">
            {activeFilterCount} active filters
          </span>
        </div>

        <div className="mt-6 space-y-6">
          <div>
            <div className="flex items-center justify-between gap-3">
              <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">
                Brand
              </p>
              {selectedBrand ? (
                <span className="text-xs font-medium uppercase tracking-[0.18em] text-stone-500">
                  {selectedBrand.brand}
                </span>
              ) : null}
            </div>
            <div className="mt-4 flex flex-wrap gap-2">
              <Link
                className={`rounded-full border px-4 py-2 text-sm transition ${selectedBrandId ? "border-stone-200 text-stone-600 hover:border-stone-900 hover:text-stone-950" : "border-orange-300 bg-orange-50 text-stone-950"}`}
                href={buildCatalogHref(undefined, selectedTypeId)}
              >
                All Brands
              </Link>
              {brands.map((brand) => (
                <Link
                  key={brand.id}
                  className={`rounded-full border px-4 py-2 text-sm transition ${selectedBrandId === brand.id ? "border-orange-300 bg-orange-50 text-stone-950" : "border-stone-200 text-stone-600 hover:border-stone-900 hover:text-stone-950"}`}
                  href={buildCatalogHref(brand.id, selectedTypeId)}
                >
                  {brand.brand}
                </Link>
              ))}
            </div>
          </div>

          <div>
            <div className="flex items-center justify-between gap-3">
              <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">
                Type
              </p>
              {selectedItemType ? (
                <span className="text-xs font-medium uppercase tracking-[0.18em] text-stone-500">
                  {selectedItemType.type}
                </span>
              ) : null}
            </div>
            <div className="mt-4 flex flex-wrap gap-2">
              <Link
                className={`rounded-full border px-4 py-2 text-sm transition ${selectedTypeId ? "border-stone-200 text-stone-600 hover:border-stone-900 hover:text-stone-950" : "border-orange-300 bg-orange-50 text-stone-950"}`}
                href={buildCatalogHref(selectedBrandId, undefined)}
              >
                All Types
              </Link>
              {itemTypes.map((itemType) => (
                <Link
                  key={itemType.id}
                  className={`rounded-full border px-4 py-2 text-sm transition ${selectedTypeId === itemType.id ? "border-orange-300 bg-orange-50 text-stone-950" : "border-stone-200 text-stone-600 hover:border-stone-900 hover:text-stone-950"}`}
                  href={buildCatalogHref(selectedBrandId, itemType.id)}
                >
                  {itemType.type}
                </Link>
              ))}
            </div>
          </div>

          <Link
            className="inline-flex w-full items-center justify-center rounded-full border border-stone-200 px-4 py-3 text-sm font-semibold uppercase tracking-[0.18em] text-stone-700 transition hover:border-stone-900 hover:text-stone-950"
            href="/salesitem"
          >
            Clear all filters
          </Link>
        </div>
      </aside>

      <div className="flex min-w-0 flex-col gap-6">
        <div className="rounded-[1.9rem] border border-stone-200 bg-white px-6 py-5 shadow-[0_14px_32px_rgba(90,67,42,0.08)]">
          <div className="flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
            <div>
              <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">
                Product Grid
              </p>
              <h3 className="mt-2 text-2xl font-semibold tracking-tight text-stone-950">
                {catalog.count} items ready to browse
              </h3>
              <p className="mt-3 max-w-2xl text-sm leading-7 text-stone-600">
                Commerce stays in the main lane while filtering remains anchored at the side.
              </p>
            </div>

            <div className="flex flex-wrap gap-2">
              <span className="rounded-full border border-stone-200 px-3 py-2 text-xs font-semibold uppercase tracking-[0.18em] text-stone-700">
                {selectedBrand?.brand ?? "All brands"}
              </span>
              <span className="rounded-full border border-stone-200 px-3 py-2 text-xs font-semibold uppercase tracking-[0.18em] text-stone-700">
                {selectedItemType?.type ?? "All types"}
              </span>
            </div>
          </div>
        </div>

        {catalog.data.length === 0 ? (
          <div className="rounded-[1.9rem] border border-dashed border-stone-300 bg-stone-50 p-8 text-stone-600">
            No items matched the current catalog filters.
          </div>
        ) : (
          <div className="grid gap-5 md:grid-cols-2 2xl:grid-cols-3">
            {catalog.data.map((salesitem) => {
              const salesitemHref = `/salesitem/${salesitem.id}`;
              const salesitemSummary = buildCatalogSummary(salesitem.description);

              return (
                <article
                  key={salesitem.id}
                  className="group rounded-[1.6rem] border border-stone-200 bg-white p-6 shadow-[0_14px_32px_rgba(90,67,42,0.08)] transition hover:-translate-y-1 hover:border-orange-300 hover:shadow-[0_20px_40px_rgba(90,67,42,0.12)]"
                >
                  <Link className="block" href={salesitemHref}>
                    <div className="overflow-hidden rounded-[1.2rem] bg-stone-100">
                      <img
                        alt={salesitem.name}
                        className="aspect-4/3 w-full object-cover transition duration-300 group-hover:scale-[1.03]"
                        src={salesitem.pictureUrl}
                      />
                    </div>
                    <div className="mt-5 flex items-center justify-between gap-3 text-xs font-semibold uppercase tracking-[0.24em] text-orange-500">
                      <span>{salesitem.salesItemBrand.brand}</span>
                      <span>{salesitem.salesItemType.type}</span>
                    </div>
                    <h3 className="mt-4 text-2xl font-semibold tracking-tight text-stone-950 transition group-hover:text-orange-600">
                      {salesitem.name}
                    </h3>
                    <p className="mt-3 min-h-21 text-sm leading-7 text-stone-600">{salesitemSummary}</p>
                  </Link>
                  <div className="mt-5 flex items-center justify-between gap-4">
                    <p className="text-lg font-semibold text-stone-950">
                      {formatPrice(salesitem.basePrice, salesitem.baseCurrencyCode)}
                    </p>
                    <Link
                      className="text-sm font-semibold tracking-[0.16em] text-stone-500 transition hover:text-orange-600"
                      href={salesitemHref}
                    >
                      View item
                    </Link>
                  </div>
                </article>
              );
            })}
          </div>
        )}
      </div>
    </section>
  );
}
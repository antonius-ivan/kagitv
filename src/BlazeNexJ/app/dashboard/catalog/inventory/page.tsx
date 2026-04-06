import TwDefaultLayout from "../../../aacomponents/layouts/TwDefaultLayout";

export default function DashboardCatalogInventoryPage() {
  return (
    <TwDefaultLayout
      eyebrow="Catalog Inventory"
      title="Inventory now appears as its own dashboard destination."
      description="The menu tree can now show inventory beneath catalog while this route holds the place for future stock and availability views."
    >
      <section className="rounded-[1.6rem] border border-stone-200 bg-white p-8 shadow-[0_14px_32px_rgba(90,67,42,0.08)]">
        <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">Catalog</p>
        <h3 className="mt-3 text-2xl font-semibold tracking-tight text-stone-950">Inventory workspace placeholder</h3>
        <p className="mt-4 max-w-3xl text-sm leading-7 text-stone-600">
          This route is the next landing point for stock thresholds, replenishment status, and inventory exceptions.
        </p>
      </section>
    </TwDefaultLayout>
  );
}
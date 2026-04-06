import TwDefaultLayout from "../../../aacomponents/layouts/TwDefaultLayout";

export default function DashboardCatalogPricingPage() {
  return (
    <TwDefaultLayout
      eyebrow="Catalog Pricing"
      title="Pricing is now surfaced as a distinct catalog branch."
      description="This placeholder route exists so the dashboard menu can show a fuller navigation tree without leading to dead links."
    >
      <section className="rounded-[1.6rem] border border-stone-200 bg-white p-8 shadow-[0_14px_32px_rgba(90,67,42,0.08)]">
        <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">Catalog</p>
        <h3 className="mt-3 text-2xl font-semibold tracking-tight text-stone-950">Pricing workspace placeholder</h3>
        <p className="mt-4 max-w-3xl text-sm leading-7 text-stone-600">
          Future price lists, promotions, and rule-based pricing controls can be anchored here without changing the dashboard drawer contract.
        </p>
      </section>
    </TwDefaultLayout>
  );
}
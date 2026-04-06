import TwDefaultLayout from "../../aacomponents/layouts/TwDefaultLayout";
import { getSession } from "../../aacomponents/server/auth";
import { formatCatalogPrice, getCatalogItem } from "../../aacomponents/server/catalog";
import Link from "next/link";
import { notFound } from "next/navigation";

type RouteParams = { params: Promise<{ salesitem: string }> };

export const dynamic = "force-dynamic";

export default async function SalesitemDetailPage({ params }: RouteParams) {
    const { salesitem } = await params;
    const salesitemId = Number(salesitem);

    if (!Number.isInteger(salesitemId) || salesitemId <= 0) {
        notFound();
    }

    let selectedSalesitem;

    try {
        selectedSalesitem = await getCatalogItem(salesitemId);
    } catch {
        notFound();
    }

    const session = await getSession();
    const returnTo = `/salesitem/${salesitemId}`;

    return (
        <TwDefaultLayout
            eyebrow="Catalog Detail"
            title={selectedSalesitem.name}
            description="This detail page uses the live catalog contract and the identity handoff for the reduced catalog-and-login storefront baseline."
        >
            <article className="grid gap-8 rounded-[1.75rem] border border-stone-200 bg-white p-8 shadow-[0_14px_32px_rgba(90,67,42,0.08)] lg:grid-cols-[0.95fr_1.05fr]">
                <div className="overflow-hidden rounded-3xl bg-stone-100">
                    <img alt={selectedSalesitem.name} className="aspect-4/3 h-full w-full object-cover" src={selectedSalesitem.pictureUrl} />
                </div>
                <div>
                    <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">Sales Item Overview</p>
                    <h3 className="mt-4 text-3xl font-semibold tracking-tight text-stone-950">{selectedSalesitem.name}</h3>
                    <p className="mt-4 max-w-3xl text-base leading-8 text-stone-700">{selectedSalesitem.description}</p>
                    <dl className="mt-6 grid gap-4 text-sm text-stone-600 sm:grid-cols-2">
                        <div className="rounded-[1.25rem] border border-stone-200 bg-stone-50 p-4">
                            <dt className="font-semibold uppercase tracking-[0.2em] text-stone-500">Brand</dt>
                            <dd className="mt-2 text-base font-semibold text-stone-950">{selectedSalesitem.salesItemBrand.brand}</dd>
                        </div>
                        <div className="rounded-[1.25rem] border border-stone-200 bg-stone-50 p-4">
                            <dt className="font-semibold uppercase tracking-[0.2em] text-stone-500">Type</dt>
                            <dd className="mt-2 text-base font-semibold text-stone-950">{selectedSalesitem.salesItemType.type}</dd>
                        </div>
                    </dl>
                    <p className="mt-6 text-3xl font-semibold tracking-tight text-stone-950">
                        {formatCatalogPrice(selectedSalesitem.basePrice, selectedSalesitem.baseCurrencyCode)}
                    </p>
                    <div className="mt-8 flex flex-wrap items-center gap-3">
                        {session ? (
                            <Link
                                className="rounded-full bg-stone-950 px-5 py-3 text-sm font-semibold tracking-[0.18em] text-white transition hover:bg-orange-500"
                                href="/dashboard"
                            >
                                Open Dashboard Menu
                            </Link>
                        ) : (
                            <a
                                className="rounded-full bg-stone-950 px-5 py-3 text-sm font-semibold tracking-[0.18em] text-white transition hover:bg-orange-500"
                                href={`/auth/login?returnTo=${encodeURIComponent(returnTo)}`}
                            >
                                Log In To Continue
                            </a>
                        )}
                        <p className="text-sm font-medium text-stone-600">
                            Purchasing, basket, and checkout flows are outside the current downspecced storefront baseline.
                        </p>
                    </div>
                </div>
                <div className="flex flex-wrap gap-3 lg:col-span-2">
                    <Link
                        className="rounded-full border border-stone-300 px-5 py-3 text-sm font-semibold tracking-[0.18em] text-stone-700 transition hover:border-stone-950 hover:text-stone-950"
                        href="/salesitem"
                    >
                        Back To Catalog
                    </Link>
                    <Link
                        className="rounded-full border border-orange-300 bg-orange-50 px-5 py-3 text-sm font-semibold tracking-[0.18em] text-stone-900 transition hover:border-orange-500 hover:bg-orange-100"
                        href="/auth"
                    >
                        Review Sign-In
                    </Link>
                </div>
            </article>
        </TwDefaultLayout>
    );
}
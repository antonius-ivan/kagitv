import Link from "next/link";

const destinations = [
  {
    href: "/dashboard",
    eyebrow: "Operations",
    title: "Open dashboard",
    description: "Go to the shared admin surface for reports, inventory, pricing, and role-based navigation.",
    accentClassName: "bg-stone-950 text-white hover:bg-orange-500",
    borderClassName: "border-stone-900/10 bg-white"
  },
  {
    href: "/salesitem",
    eyebrow: "Commerce",
    title: "Open salesitem",
    description: "Jump into the storefront catalog and continue browsing the live SalesItem experience.",
    accentClassName: "bg-orange-500 text-white hover:bg-stone-950",
    borderClassName: "border-orange-200 bg-orange-50/70"
  }
] as const;

export default function TransitChooser() {
  return (
    <div className="grid gap-5 lg:grid-cols-2">
      {destinations.map((destination) => (
        <article
          key={destination.href}
          className={`rounded-[1.8rem] border p-6 shadow-[0_18px_40px_rgba(73,37,0,0.08)] transition hover:-translate-y-0.5 ${destination.borderClassName}`}
        >
          <p className="text-xs font-semibold uppercase tracking-[0.3em] text-stone-500">
            {destination.eyebrow}
          </p>
          <h3 className="mt-4 text-2xl font-semibold tracking-tight text-stone-950">
            {destination.title}
          </h3>
          <p className="mt-3 max-w-xl text-sm leading-7 text-stone-600">
            {destination.description}
          </p>
          <div className="mt-8 flex items-center justify-between gap-4">
            <span className="text-xs font-semibold uppercase tracking-[0.24em] text-stone-400">
              Route {destination.href}
            </span>
            <Link
              className={`inline-flex items-center rounded-[1.1rem] px-5 py-3 text-sm font-semibold tracking-tight transition ${destination.accentClassName}`}
              href={destination.href}
            >
              Continue
            </Link>
          </div>
        </article>
      ))}
    </div>
  );
}
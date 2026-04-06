import TwDefaultLayout from "../../aacomponents/layouts/TwDefaultLayout";
import { getSession } from "../../aacomponents/server/auth";
import { notFound, redirect } from "next/navigation";

export default async function DashboardReportsPage() {
  const session = await getSession();

  if (!session) {
    redirect("/auth/login?returnTo=/dashboard/reports");
  }

  if (session.user.roles?.includes("Admin") !== true) {
    notFound();
  }

  return (
    <TwDefaultLayout
      eyebrow="Reports"
      title="Admin reporting now hangs off the role-aware menu tree."
      description="This route is intended for admin-focused ERP reporting expansion and is seeded only for the Admin role in the initial UI control layer."
    >
      <section className="rounded-[1.6rem] border border-stone-200 bg-white p-8 shadow-[0_14px_32px_rgba(90,67,42,0.08)]">
        <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">Admin Module</p>
        <h3 className="mt-3 text-2xl font-semibold tracking-tight text-stone-950">Reports are reserved for the admin branch</h3>
        <p className="mt-4 max-w-3xl text-sm leading-7 text-stone-600">
          Staff users will not receive this menu entry from the backend. This page is the next attachment point for finance and operations summaries once the reporting scope is approved.
        </p>
      </section>
    </TwDefaultLayout>
  );
}
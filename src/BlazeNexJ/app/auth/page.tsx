import TwDefaultLayout from "../aacomponents/layouts/TwDefaultLayout";
import { getSession } from "../aacomponents/server/auth";

export const dynamic = "force-dynamic";

export default async function AuthPage() {
  const session = await getSession();

  return (
    <TwDefaultLayout
      eyebrow="Identity"
      title="Sign in through the Glaive identity service."
      description="This route starts the OpenID Connect flow used for the reduced BlazeNexJ catalog and dashboard baseline."
    >
      <section className="rounded-[1.6rem] border border-stone-200 bg-white p-8 shadow-[0_14px_32px_rgba(90,67,42,0.08)]">
        {session ? (
          <div className="space-y-4">
            <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">Signed In</p>
            <h3 className="text-2xl font-semibold tracking-tight text-stone-950">{session.user.name}</h3>
            <p className="max-w-3xl text-base leading-8 text-stone-600">
              The auth foundation is active. You can now use identity-backed catalog and dashboard routes inside BlazeNexJ.
            </p>
            <a
              className="inline-flex rounded-full bg-stone-950 px-5 py-3 text-sm font-semibold tracking-[0.18em] text-white transition hover:bg-orange-500"
              href="/auth/logout"
            >
              Log Out
            </a>
          </div>
        ) : (
          <div className="space-y-4">
            <p className="text-sm font-semibold uppercase tracking-[0.24em] text-orange-500">Sign In Required</p>
            <h3 className="text-2xl font-semibold tracking-tight text-stone-950">Connect BlazeNexJ to Identity.API</h3>
            <p className="max-w-3xl text-base leading-8 text-stone-600">
              Catalog sign-in handoff and dashboard access depend on the same session foundation. Start the authorization-code flow here.
            </p>
            <a
              className="inline-flex rounded-full bg-stone-950 px-5 py-3 text-sm font-semibold tracking-[0.18em] text-white transition hover:bg-orange-500"
              href="/auth/login?returnTo=/salesitem"
            >
              Sign In
            </a>
          </div>
        )}
      </section>
    </TwDefaultLayout>
  );
}
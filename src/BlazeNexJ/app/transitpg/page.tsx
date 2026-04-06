import TransitChooser from "../aacomponents/TransitChooser";
import SharedAppFrame from "../aacomponents/layouts/SharedAppFrame";
import SharedTopBar from "../aacomponents/navigation/SharedTopBar";

export default async function TransitPage() {
  return (
    <SharedAppFrame
      header={
        <SharedTopBar
          section="Transit"
          title="Choose your next route"
          subtitle="Move into the dashboard or the storefront without changing the existing route behavior."
        />
      }
      intro={
        <>
          <p className="text-xs font-semibold uppercase tracking-[0.34em] text-orange-500">Entry</p>
          <h1 className="mt-3 max-w-4xl text-4xl font-semibold tracking-tight text-stone-950">
            Start from a single transit page.
          </h1>
          <p className="mt-4 max-w-3xl text-base leading-8 text-stone-600">
            Use this handoff page to choose whether you want the operational dashboard or the salesitem catalog.
          </p>
        </>
      }
    >
      <TransitChooser />
    </SharedAppFrame>
  );
}
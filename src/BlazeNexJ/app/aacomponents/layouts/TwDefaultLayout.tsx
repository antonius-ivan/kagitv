import type { ReactNode } from "react";
import SharedAppFrame from "./SharedAppFrame";
import SharedTopBar from "../navigation/SharedTopBar";

type TwDefaultLayoutProps = {
  children: ReactNode;
  eyebrow: string;
  title: string;
  description: string;
  search?: {
    action: string;
    placeholder: string;
    defaultValue?: string;
    hiddenFields?: Array<{ name: string; value: string }>;
  };
};

export default async function TwDefaultLayout({
  children,
  eyebrow,
  title,
  description,
  search = {
    action: "/salesitem",
    placeholder: "Search the salesitem catalog"
  }
}: TwDefaultLayoutProps) {
  return (
    <SharedAppFrame
      header={
        <SharedTopBar
          search={search}
          section="Commerce"
          title="Glaive storefront"
          utilities={{
            left: { title: "Category" },
            right: { title: "Address / Convenience" }
          }}
        />
      }
      intro={
        <>
          <p className="text-xs font-semibold uppercase tracking-[0.34em] text-orange-500">{eyebrow}</p>
          <h2 className="mt-3 max-w-4xl text-4xl font-semibold tracking-tight text-stone-950">
            {title}
          </h2>
          <p className="mt-4 max-w-3xl text-base leading-8 text-stone-600">{description}</p>
        </>
      }
    >
      {children}
    </SharedAppFrame>
  );
}
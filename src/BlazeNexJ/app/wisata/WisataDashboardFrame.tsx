import type { ReactNode } from "react";
import SharedAppFrame from "../aacomponents/layouts/SharedAppFrame";
import SharedTopBar from "../aacomponents/navigation/SharedTopBar";
import { getRoleMenu } from "../aacomponents/server/menu";
import FluentDashboardLayout from "../afluentcomponents/FluentDashboardLayout";
import FluentProviderRegistry from "../afluentcomponents/FluentProviderRegistry";

type WisataDashboardFrameProps = {
  title: string;
  description: string;
  children: ReactNode;
};

export default async function WisataDashboardFrame({
  title,
  description,
  children
}: WisataDashboardFrameProps) {
  const modules = await getRoleMenu();

  return (
    <SharedAppFrame
      contentClassName="pt-0"
      header={
        <SharedTopBar
          section="Wisata"
          title="Glaive tourism workspace"
          subtitle="Shared Fluent UI shell"
        />
      }
    >
      <FluentProviderRegistry>
        <FluentDashboardLayout
          modules={modules}
          eyebrow="Wisata Module"
          title={title}
          description={description}
        >
          {children}
        </FluentDashboardLayout>
      </FluentProviderRegistry>
    </SharedAppFrame>
  );
}
import WisataDashboardFrame from "./WisataDashboardFrame";
import WisataListClient from "./WisataListClient";

export default function WisataListPage() {
  return (
    <WisataDashboardFrame
      title="Operate Wisata inside the shared dashboard shell."
      description="The Wisata list now runs inside the Fluent navigation frame so the tourism module sits beside the rest of the workspace instead of opening its own shell."
    >
      <WisataListClient />
    </WisataDashboardFrame>
  );
}
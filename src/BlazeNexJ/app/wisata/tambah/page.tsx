import WisataDashboardFrame from "../WisataDashboardFrame";
import WisataFormPage from "../WisataFormPage";

export default function TambahWisataPage() {
  return (
    <WisataDashboardFrame
      title="Create Wisata records without leaving the dashboard."
      description="The add flow now stays inside the shared Fluent shell while preserving the same backend create behavior and Wisata auth checks."
    >
      <WisataFormPage mode="create" />
    </WisataDashboardFrame>
  );
}
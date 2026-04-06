import WisataDashboardFrame from "../../WisataDashboardFrame";
import WisataFormPage from "../../WisataFormPage";

type EditWisataPageProps = {
  params: Promise<{ id: string }>;
};

export default async function EditWisataPage({ params }: EditWisataPageProps) {
  const { id } = await params;
  return (
    <WisataDashboardFrame
      title="Update Wisata data inside the shared workspace."
      description="Editing stays in the same Fluent dashboard frame, so route changes no longer swap the user into a separate Wisata-only shell."
    >
      <WisataFormPage mode="edit" wisataId={id} />
    </WisataDashboardFrame>
  );
}
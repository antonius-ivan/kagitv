import WisataDashboardFrame from "../WisataDashboardFrame";
import WisataReportClient from "../WisataReportClient";

export default function WisataReportPage() {
  return (
    <WisataDashboardFrame
      title="Review Wisata reports from the same Fluent workspace."
      description="The report page stays inside the shared dashboard shell while keeping the existing export-to-PDF flow and Wisata API contract unchanged."
    >
      <WisataReportClient />
    </WisataDashboardFrame>
  );
}
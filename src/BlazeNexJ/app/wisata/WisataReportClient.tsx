"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { Button, makeStyles, tokens } from "@fluentui/react-components";
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import type { WisataReportResponse } from "@/app/types";
import { getWisataSession, wisataApiFetch } from "./auth-client";
import { formatRupiah, formatTanggalCetak } from "./utils";

const useStyles = makeStyles({
  panel: {
    border: `1px solid ${tokens.colorNeutralStroke2}`,
    borderRadius: tokens.borderRadiusXLarge,
    backgroundColor: tokens.colorNeutralBackground1,
    boxShadow: "0 20px 48px rgba(15, 23, 42, 0.08)",
    padding: tokens.spacingHorizontalXXL,
    display: "grid",
    gap: tokens.spacingVerticalL,
    width: "100%"
  },
  actionRow: {
    display: "flex",
    flexWrap: "wrap",
    gap: tokens.spacingHorizontalS
  },
  error: {
    borderRadius: tokens.borderRadiusLarge,
    backgroundColor: tokens.colorPaletteRedBackground1,
    color: tokens.colorPaletteRedForeground1,
    padding: `${tokens.spacingVerticalS} ${tokens.spacingHorizontalM}`,
    fontSize: tokens.fontSizeBase200
  },
  info: {
    color: tokens.colorNeutralForeground3,
    fontSize: tokens.fontSizeBase200
  },
  tableWrap: {
    overflowX: "auto",
    borderRadius: tokens.borderRadiusXLarge,
    border: `1px solid ${tokens.colorNeutralStroke2}`
  },
  table: {
    width: "100%",
    borderCollapse: "collapse"
  },
  headerCell: {
    textAlign: "left",
    padding: `${tokens.spacingVerticalM} ${tokens.spacingHorizontalM}`,
    backgroundColor: tokens.colorNeutralBackground3,
    color: tokens.colorNeutralForeground1,
    fontWeight: tokens.fontWeightSemibold,
    fontSize: tokens.fontSizeBase200,
    borderBottom: `1px solid ${tokens.colorNeutralStroke2}`
  },
  cell: {
    padding: `${tokens.spacingVerticalM} ${tokens.spacingHorizontalM}`,
    borderBottom: `1px solid ${tokens.colorNeutralStroke2}`,
    color: tokens.colorNeutralForeground2,
    fontSize: tokens.fontSizeBase200,
    verticalAlign: "top"
  },
  strongCell: {
    color: tokens.colorNeutralForeground1,
    fontWeight: tokens.fontWeightSemibold
  },
  summary: {
    display: "grid",
    gap: tokens.spacingVerticalS,
    borderTop: `1px solid ${tokens.colorNeutralStroke2}`,
    padding: `${tokens.spacingVerticalL} ${tokens.spacingHorizontalM}`,
    color: tokens.colorNeutralForeground1,
    fontWeight: tokens.fontWeightSemibold
  }
});

export default function WisataReportClient() {
  const classes = useStyles();
  const router = useRouter();
  const [report, setReport] = useState<WisataReportResponse | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!getWisataSession()) {
      router.replace("/wisata/login");
      return;
    }

    let isCancelled = false;

    async function loadReport() {
      try {
        const response = await wisataApiFetch("/api/wisata/report");
        if (!response.ok) {
          throw new Error("Laporan wisata gagal dimuat.");
        }

        const payload = await response.json() as WisataReportResponse;
        if (!isCancelled) {
          setReport(payload);
        }
      } catch (loadError) {
        if (!isCancelled) {
          if (loadError instanceof Error && loadError.message === "UNAUTHORIZED") {
            router.replace("/wisata/login");
            return;
          }

          setError(loadError instanceof Error ? loadError.message : "Laporan wisata gagal dimuat.");
        }
      } finally {
        if (!isCancelled) {
          setLoading(false);
        }
      }
    }

    void loadReport();

    return () => {
      isCancelled = true;
    };
  }, [router]);

  function handleExportPdf() {
    if (!report) {
      return;
    }

    const pdf = new jsPDF({ unit: "pt", format: "a4" });
    pdf.setFontSize(18);
    pdf.text("LAPORAN DATA WISATA", 40, 50);

    autoTable(pdf, {
      startY: 80,
      head: [["WisataID", "Nama", "Kota", "Harga (Rp)"]],
      body: report.items.map((item) => [item.wisataId, item.nama, item.kota, formatRupiah(item.harga)]),
      headStyles: {
        fillColor: [14, 116, 144]
      },
      styles: {
        fontSize: 10
      }
    });

    const finalY = (pdf as jsPDF & { lastAutoTable?: { finalY: number } }).lastAutoTable?.finalY ?? 100;
    pdf.setFontSize(12);
    pdf.text(`TOTAL: ${formatRupiah(report.totalHarga)}`, 40, finalY + 30);
    pdf.text(`Tanggal Cetak: ${formatTanggalCetak(report.printedAt)}`, 40, finalY + 50);
    pdf.save("laporan-data-wisata.pdf");
  }

  return (
    <section className={classes.panel}>
      <div className={classes.actionRow}>
        <Button appearance="primary" disabled={!report} onClick={handleExportPdf}>Export PDF</Button>
        <Button appearance="secondary" onClick={() => router.push("/wisata")}>Kembali ke daftar</Button>
      </div>

      {loading ? <p className={classes.info}>Memuat laporan wisata...</p> : null}
      {error ? <p className={classes.error}>{error}</p> : null}

      {report ? (
        <div className={classes.tableWrap}>
          <table className={classes.table}>
            <thead>
              <tr>
                <th className={classes.headerCell}>WisataID</th>
                <th className={classes.headerCell}>Nama</th>
                <th className={classes.headerCell}>Kota</th>
                <th className={classes.headerCell}>Harga (Rp)</th>
              </tr>
            </thead>
            <tbody>
              {report.items.map((item) => (
                <tr key={item.wisataId}>
                  <td className={classes.cell}>{item.wisataId}</td>
                  <td className={`${classes.cell} ${classes.strongCell}`}>{item.nama}</td>
                  <td className={classes.cell}>{item.kota}</td>
                  <td className={classes.cell}>{formatRupiah(item.harga)}</td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className={classes.summary}>
            <p>TOTAL: {formatRupiah(report.totalHarga)}</p>
            <p>Tanggal Cetak: {formatTanggalCetak(report.printedAt)}</p>
          </div>
        </div>
      ) : null}
    </section>
  );
}
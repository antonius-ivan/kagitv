"use client";

import Link from "next/link";
import { useDeferredValue, useEffect, useState, useTransition } from "react";
import { useRouter } from "next/navigation";
import { Button, makeStyles, tokens } from "@fluentui/react-components";
import type { WisataItem, WisataListResponse } from "@/app/types";
import { getWisataSession, logoutWisata, wisataApiFetch } from "./auth-client";
import { formatRupiah } from "./utils";

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
  toolbar: {
    display: "flex",
    flexWrap: "wrap",
    gap: tokens.spacingHorizontalL,
    justifyContent: "space-between",
    alignItems: "end"
  },
  identity: {
    display: "grid",
    gap: tokens.spacingVerticalXS
  },
  label: {
    color: tokens.colorNeutralForeground3,
    fontSize: tokens.fontSizeBase200
  },
  value: {
    color: tokens.colorNeutralForeground1,
    fontSize: tokens.fontSizeBase500,
    fontWeight: tokens.fontWeightSemibold
  },
  searchWrap: {
    width: "min(100%, 28rem)"
  },
  searchLabel: {
    display: "block",
    marginBottom: tokens.spacingVerticalXS,
    color: tokens.colorNeutralForeground2,
    fontSize: tokens.fontSizeBase200,
    fontWeight: tokens.fontWeightMedium
  },
  searchInput: {
    width: "100%",
    borderRadius: tokens.borderRadiusLarge,
    border: `1px solid ${tokens.colorNeutralStroke2}`,
    padding: `${tokens.spacingVerticalMNudge} ${tokens.spacingHorizontalM}`,
    backgroundColor: tokens.colorNeutralBackground2,
    color: tokens.colorNeutralForeground1,
    outlineStyle: "none",
    fontSize: tokens.fontSizeBase300,
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
  emptyCell: {
    textAlign: "center",
    color: tokens.colorNeutralForeground3,
    padding: `${tokens.spacingVerticalXXL} ${tokens.spacingHorizontalM}`
  },
  pagination: {
    display: "flex",
    flexWrap: "wrap",
    gap: tokens.spacingHorizontalL,
    justifyContent: "space-between",
    alignItems: "center"
  },
  paginationText: {
    color: tokens.colorNeutralForeground3,
    fontSize: tokens.fontSizeBase200
  },
  paginationButtons: {
    display: "flex",
    alignItems: "center",
    gap: tokens.spacingHorizontalS
  },
  pageValue: {
    minWidth: "5rem",
    textAlign: "center",
    color: tokens.colorNeutralForeground2,
    fontWeight: tokens.fontWeightMedium
  },
  linkButton: {
    display: "inline-flex",
    alignItems: "center",
    justifyContent: "center",
    minHeight: "32px",
    padding: `0 ${tokens.spacingHorizontalM}`,
    borderRadius: tokens.borderRadiusLarge,
    border: `1px solid ${tokens.colorBrandStroke1}`,
    color: tokens.colorBrandForeground1,
    textDecoration: "none",
    fontWeight: tokens.fontWeightMedium,
    backgroundColor: tokens.colorNeutralBackground1
  }
});

export default function WisataListClient() {
  const classes = useStyles();
  const router = useRouter();
  const [session, setSession] = useState<ReturnType<typeof getWisataSession> | undefined>(undefined);
  const [search, setSearch] = useState("");
  const deferredSearch = useDeferredValue(search);
  const [page, setPage] = useState(1);
  const [data, setData] = useState<WisataListResponse | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [isPending, startTransition] = useTransition();

  useEffect(() => {
    setSession(getWisataSession());
  }, []);

  useEffect(() => {
    if (session === undefined) {
      return;
    }

    if (!session?.accessToken) {
      router.replace("/wisata/login");
      return;
    }

    let isCancelled = false;

    async function loadData() {
      setLoading(true);
      setError(null);

      try {
        const query = new URLSearchParams({
          page: String(page),
          pageSize: "5"
        });

        if (deferredSearch.trim()) {
          query.set("search", deferredSearch.trim());
        }

        const response = await wisataApiFetch(`/api/wisata?${query.toString()}`);
        if (!response.ok) {
          throw new Error("Gagal memuat data wisata.");
        }

        const payload = await response.json() as WisataListResponse;
        if (!isCancelled) {
          setData(payload);
        }
      } catch (loadError) {
        if (!isCancelled) {
          if (loadError instanceof Error && loadError.message === "UNAUTHORIZED") {
            router.replace("/wisata/login");
            return;
          }

          setError(loadError instanceof Error ? loadError.message : "Gagal memuat data wisata.");
        }
      } finally {
        if (!isCancelled) {
          setLoading(false);
        }
      }
    }

    void loadData();

    return () => {
      isCancelled = true;
    };
  }, [deferredSearch, page, router, session]);

  async function handleDelete(item: WisataItem) {
    const shouldDelete = window.confirm(`Apakah Yakin Delete Data \"${item.nama}\"?`);
    if (!shouldDelete) {
      return;
    }

    try {
      const response = await wisataApiFetch(`/api/wisata/${item.wisataId}`, {
        method: "DELETE"
      });

      if (!response.ok) {
        throw new Error("Data gagal dihapus.");
      }

      startTransition(() => {
        setPage((currentPage) => currentPage);
      });

      const refreshResponse = await wisataApiFetch(`/api/wisata?page=${page}&pageSize=5${deferredSearch.trim() ? `&search=${encodeURIComponent(deferredSearch.trim())}` : ""}`);
      const payload = await refreshResponse.json() as WisataListResponse;
      setData(payload);
    } catch (deleteError) {
      if (deleteError instanceof Error && deleteError.message === "UNAUTHORIZED") {
        router.replace("/wisata/login");
        return;
      }

      setError(deleteError instanceof Error ? deleteError.message : "Data gagal dihapus.");
    }
  }

  async function handleLogout() {
    await logoutWisata();
    setSession(null);
    router.replace("/wisata/login");
  }

  const totalPages = data ? Math.max(1, Math.ceil(data.totalCount / data.pageSize)) : 1;

  return (
    <section className={classes.panel}>
      <div className={classes.toolbar}>
        <div className={classes.identity}>
          <span className={classes.label}>Login sebagai</span>
          <span className={classes.value}>{session?.username ?? "Memeriksa sesi..."}</span>
        </div>

        <div className={classes.searchWrap}>
          <label className={classes.searchLabel} htmlFor="wisata-search">Search by Nama</label>
          <input
            id="wisata-search"
            className={classes.searchInput}
            placeholder="Cari wisata berdasarkan nama"
            value={search}
            onChange={(event) => {
              const nextValue = event.target.value;
              startTransition(() => {
                setSearch(nextValue);
                setPage(1);
              });
            }}
          />
        </div>
      </div>

      <div className={classes.actionRow}>
        <Button appearance="primary" onClick={() => router.push("/wisata/tambah")}>Tambah Data</Button>
        <Button appearance="secondary" onClick={() => router.push("/wisata/laporan")}>Laporan</Button>
        <Button appearance="subtle" onClick={() => void handleLogout()}>Logout</Button>
      </div>

      {error ? <p className={classes.error}>{error}</p> : null}

      <div className={classes.tableWrap}>
        <table className={classes.table}>
          <thead>
            <tr>
              <th className={classes.headerCell}>ID</th>
              <th className={classes.headerCell}>Nama</th>
              <th className={classes.headerCell}>Kota</th>
              <th className={classes.headerCell}>Harga</th>
              <th className={classes.headerCell}>Aksi</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td className={classes.emptyCell} colSpan={5}>Memuat data wisata...</td>
              </tr>
            ) : data && data.items.length > 0 ? (
              data.items.map((item) => (
                <tr key={item.wisataId}>
                  <td className={classes.cell}>{item.wisataId}</td>
                  <td className={`${classes.cell} ${classes.strongCell}`}>{item.nama}</td>
                  <td className={classes.cell}>{item.kota}</td>
                  <td className={classes.cell}>{formatRupiah(item.harga)}</td>
                  <td className={classes.cell}>
                    <div className={classes.actionRow}>
                      <Link className={classes.linkButton} href={`/wisata/${item.wisataId}/edit`}>
                        Edit
                      </Link>
                      <Button appearance="secondary" onClick={() => void handleDelete(item)}>
                        Delete
                      </Button>
                    </div>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td className={classes.emptyCell} colSpan={5}>Belum ada data wisata yang cocok.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      <div className={classes.pagination}>
        <p className={classes.paginationText}>Menampilkan maksimal 5 data per halaman.</p>
        <div className={classes.paginationButtons}>
          <Button
            appearance="secondary"
            disabled={page <= 1 || loading || isPending}
            onClick={() => setPage((currentPage) => Math.max(1, currentPage - 1))}
          >
            Prev
          </Button>
          <span className={classes.pageValue}>{page} / {totalPages}</span>
          <Button
            appearance="secondary"
            disabled={page >= totalPages || loading || isPending}
            onClick={() => setPage((currentPage) => currentPage + 1)}
          >
            Next
          </Button>
        </div>
      </div>
    </section>
  );
}
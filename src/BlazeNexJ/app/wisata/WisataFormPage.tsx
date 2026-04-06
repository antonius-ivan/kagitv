"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { Button, makeStyles, tokens } from "@fluentui/react-components";
import type { WisataItem, WisataUpsertInput } from "@/app/types";
import { getWisataSession, wisataApiFetch } from "./auth-client";

type WisataFormPageProps = {
  mode: "create" | "edit";
  wisataId?: string;
};

const useStyles = makeStyles({
  panel: {
    border: `1px solid ${tokens.colorNeutralStroke2}`,
    borderRadius: tokens.borderRadiusXLarge,
    backgroundColor: tokens.colorNeutralBackground1,
    boxShadow: "0 20px 48px rgba(15, 23, 42, 0.08)",
    padding: tokens.spacingHorizontalXXL,
    display: "grid",
    gap: tokens.spacingVerticalL,
    width: "100%",
    maxWidth: "42rem"
  },
  loading: {
    color: tokens.colorNeutralForeground3,
    fontSize: tokens.fontSizeBase200
  },
  form: {
    display: "grid",
    gap: tokens.spacingVerticalL
  },
  label: {
    display: "grid",
    gap: tokens.spacingVerticalXS
  },
  caption: {
    color: tokens.colorNeutralForeground2,
    fontSize: tokens.fontSizeBase200,
    fontWeight: tokens.fontWeightMedium
  },
  input: {
    width: "100%",
    borderRadius: tokens.borderRadiusLarge,
    border: `1px solid ${tokens.colorNeutralStroke2}`,
    padding: `${tokens.spacingVerticalMNudge} ${tokens.spacingHorizontalM}`,
    backgroundColor: tokens.colorNeutralBackground2,
    color: tokens.colorNeutralForeground1,
    outlineStyle: "none",
    fontSize: tokens.fontSizeBase300,
  },
  error: {
    borderRadius: tokens.borderRadiusLarge,
    backgroundColor: tokens.colorPaletteRedBackground1,
    color: tokens.colorPaletteRedForeground1,
    padding: `${tokens.spacingVerticalS} ${tokens.spacingHorizontalM}`,
    fontSize: tokens.fontSizeBase200
  },
  actionRow: {
    display: "flex",
    flexWrap: "wrap",
    gap: tokens.spacingHorizontalS
  }
});

export default function WisataFormPage({ mode, wisataId }: WisataFormPageProps) {
  const classes = useStyles();
  const router = useRouter();
  const [form, setForm] = useState<WisataUpsertInput>({ nama: "", kota: "", harga: 0 });
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(mode === "edit");
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (!getWisataSession()) {
      router.replace("/wisata/login");
      return;
    }

    if (mode !== "edit" || !wisataId) {
      return;
    }

    let isCancelled = false;

    async function loadDetail() {
      try {
        const response = await wisataApiFetch(`/api/wisata/${wisataId}`);
        if (!response.ok) {
          throw new Error("Detail wisata tidak ditemukan.");
        }

        const payload = await response.json() as WisataItem;
        if (!isCancelled) {
          setForm({ nama: payload.nama, kota: payload.kota, harga: payload.harga });
        }
      } catch (loadError) {
        if (!isCancelled) {
          if (loadError instanceof Error && loadError.message === "UNAUTHORIZED") {
            router.replace("/wisata/login");
            return;
          }

          setError(loadError instanceof Error ? loadError.message : "Detail wisata tidak ditemukan.");
        }
      } finally {
        if (!isCancelled) {
          setLoading(false);
        }
      }
    }

    void loadDetail();

    return () => {
      isCancelled = true;
    };
  }, [mode, router, wisataId]);

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setSubmitting(true);
    setError(null);

    try {
      const response = await wisataApiFetch(mode === "create" ? "/api/wisata" : `/api/wisata/${wisataId}`, {
        method: mode === "create" ? "POST" : "PUT",
        body: JSON.stringify(form)
      });

      if (!response.ok) {
        const message = await response.text();
        throw new Error(message || "Data gagal disimpan.");
      }

      router.push("/wisata");
    } catch (submitError) {
      if (submitError instanceof Error && submitError.message === "UNAUTHORIZED") {
        router.replace("/wisata/login");
        return;
      }

      setError(submitError instanceof Error ? submitError.message : "Data gagal disimpan.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <section className={classes.panel}>
      {loading ? (
        <p className={classes.loading}>Memuat detail wisata...</p>
      ) : (
        <form className={classes.form} onSubmit={handleSubmit}>
          <label className={classes.label}>
            <span className={classes.caption}>Nama</span>
            <input
              className={classes.input}
              value={form.nama}
              onChange={(event) => setForm((currentForm) => ({ ...currentForm, nama: event.target.value }))}
              placeholder="Nama wisata"
            />
          </label>

          <label className={classes.label}>
            <span className={classes.caption}>Kota</span>
            <input
              className={classes.input}
              value={form.kota}
              onChange={(event) => setForm((currentForm) => ({ ...currentForm, kota: event.target.value }))}
              placeholder="Kota wisata"
            />
          </label>

          <label className={classes.label}>
            <span className={classes.caption}>Harga</span>
            <input
              type="number"
              min="1"
              className={classes.input}
              value={form.harga || ""}
              onChange={(event) => setForm((currentForm) => ({ ...currentForm, harga: Number(event.target.value) }))}
              placeholder="Harga tiket"
            />
          </label>

          {error ? <p className={classes.error}>{error}</p> : null}

          <div className={classes.actionRow}>
            <Button appearance="primary" disabled={submitting} type="submit">
              {submitting ? "Menyimpan..." : "Simpan"}
            </Button>
            <Button appearance="secondary" onClick={() => router.push("/wisata")} type="button">
              Batal
            </Button>
          </div>
        </form>
      )}
    </section>
  );
}
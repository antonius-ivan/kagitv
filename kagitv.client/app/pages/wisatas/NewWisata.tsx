import React, { FormEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    Button,
    Field,
    Input,
    Dialog,
    DialogSurface,
    DialogTitle,
    DialogBody,
    DialogActions,
    DialogContent,
} from "@fluentui/react-components";
import { Alert24Regular } from "@fluentui/react-icons";
import { IWisata } from "./IWisata";

// Initial empty wisata
const initialWisata: IWisata = {
    id: 0,
    wisataNumber: 0,
    wisataName: "",
    wisataAmount: 0,
    wisataPercentage: 0,
    createdDate: undefined,
    createdBy: undefined,
    updatedDate: undefined,
    updatedBy: undefined,
};

export function NewWisata(): JSX.Element {
    const [wisata, setWisata] = useState<IWisata>(initialWisata);
    const [error, setError] = useState<string | null>(null);
    const [dialogOpen, setDialogOpen] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        // Add audit fields expected by your API
        const nowIso = new Date().toISOString();
        const payload = {
            ...wisata,
            createdBy: "admin",    // ≤5 chars if your backend enforces it
            updatedBy: "admin",
            createdDate: nowIso,
            updatedDate: nowIso,
        };
        fetch("api/v1/tourney/wisatas");
        try {
            const response = await fetch(`api/v1/tourney/wisatas`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload),
            });

            if (!response.ok) {
                const detail = await response.text();
                throw new Error(`Status ${response.status}: ${detail}`);
            }

            // On success, go back to the wisata list
            navigate("/wisatalist");
        } catch (err: any) {
            setError(err.message);
            setDialogOpen(true);
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: 400, margin: "2rem auto" }}>
            <Field label="Wisata Number" required style={{ marginBottom: "1rem" }}>
                <Input
                    type="number"
                    value={wisata.wisataNumber}
                    onChange={(e) =>
                        setWisata((prev) => ({
                            ...prev,
                            wisataNumber: Number(e.target.value),
                        }))
                    }
                />
            </Field>

            <Field label="Wisata Name" required style={{ marginBottom: "1rem" }}>
                <Input
                    placeholder="Enter wisata name"
                    value={wisata.wisataName}
                    onChange={(e) =>
                        setWisata((prev) => ({ ...prev, wisataName: e.target.value }))
                    }
                />
            </Field>

            <Field label="Wisata Amount" required style={{ marginBottom: "1rem" }}>
                <Input
                    type="number"
                    placeholder="0.00"
                    value={wisata.wisataAmount}
                    onChange={(e) =>
                        setWisata((prev) => ({
                            ...prev,
                            wisataAmount: Number(e.target.value),
                        }))
                    }
                />
            </Field>

            <Field label="Wisata Percentage" required style={{ marginBottom: "1.5rem" }}>
                <Input
                    type="number"
                    placeholder="0.00"
                    value={wisata.wisataPercentage}
                    onChange={(e) =>
                        setWisata((prev) => ({
                            ...prev,
                            wisataPercentage: Number(e.target.value),
                        }))
                    }
                />
            </Field>

            <div style={{ display: "flex", gap: 8 }}>
                <Button type="submit">
                    Save
                </Button>
                <Button onClick={() => navigate("/wisatalist")}>
                    Cancel
                </Button>
            </div>

            <Dialog open={dialogOpen} onOpenChange={(_, data) => setDialogOpen(data.open)}>
                <DialogSurface>
                    <DialogTitle>
                        <Alert24Regular /> Error Saving Wisata
                    </DialogTitle>
                    <DialogBody>
                        <DialogContent>
                            We were unable to create the wisata.
                            {error && <div style={{ marginTop: 8 }}>Details: {error}</div>}
                        </DialogContent>
                    </DialogBody>
                    <DialogActions>
                        <Button onClick={() => setDialogOpen(false)}>
                            Close
                        </Button>
                    </DialogActions>
                </DialogSurface>
            </Dialog>
        </form>
    );
}

export default NewWisata;

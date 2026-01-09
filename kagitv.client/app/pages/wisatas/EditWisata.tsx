import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
    Field,
    Input,
    Button,
    Dialog,
    DialogSurface,
    DialogTitle,
    DialogBody,
    DialogContent,
    DialogActions,
    MessageBar,
} from "@fluentui/react-components";
import { IWisata } from "./IWisata";

export function EditWisata(): JSX.Element {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [wisata, setWisata] = useState<IWisata>({
        id: undefined,
        wisataNumber: 0,
        wisataName: "",
        wisataAmount: 0,
        wisataPercentage: 0,
        createdDate: undefined,
        createdBy: undefined,
        updatedDate: undefined,
        updatedBy: undefined,
    });
    const [showSuccess, setShowSuccess] = useState(false);
    const [error, setError] = useState<string>("");
    const [dialogOpen, setDialogOpen] = useState(false);

    // Fetch the existing wisata when component mounts or `id` changes
    useEffect(() => {
        async function fetchWisata() {
            const resp = await fetch(`/api/v1/tourney/wisatas/${id}`); // GET by id
            if (resp.ok) {
                const data: IWisata = await resp.json();
                setWisata(data);
            } else {
                setError(`Failed to load wisata #${id}`);
                setDialogOpen(true);
            }
        }
        if (id) {
            fetchWisata();
        }
    }, [id]);

    // Generic handler for input changes
    const handleInputChange =
        (field: keyof IWisata) =>
            (e: React.ChangeEvent<HTMLInputElement>) => {
                let val: string | number = e.target.value;
                if (
                    field === "wisataNumber" ||
                    field === "wisataAmount" ||
                    field === "wisataPercentage"
                ) {
                    val = parseFloat(val) || 0;
                }
                setWisata({ ...wisata, [field]: val });
            };

    // Submit the updated wisata via PUT (id in the URI)
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!id) return;

        // Ensure body.id matches the path id
        const body = { ...wisata, id: parseInt(id, 10) };

        const resp = await fetch(`/api/v1/tourney/wisatas/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body),
        });

        if (resp.ok) {
            setShowSuccess(true);
            setTimeout(() => navigate("/wisatalist"), 2000);
        } else {
            const text = await resp.text();
            setError(text || "Unable to update the wisata.");
            setDialogOpen(true);
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ maxWidth: 400, margin: "2rem auto" }}>
            <Field label="Wisata Number" required style={{ marginBottom: "1rem" }}>
                <Input
                    type="number"
                    placeholder="Enter wisata number"
                    value={wisata.wisataNumber}
                    onChange={handleInputChange("wisataNumber")}
                />
            </Field>

            <Field label="Wisata Name" required style={{ marginBottom: "1rem" }}>
                <Input
                    placeholder="Enter wisata name"
                    value={wisata.wisataName}
                    onChange={handleInputChange("wisataName")}
                />
            </Field>

            <Field label="Amount" required style={{ marginBottom: "1rem" }}>
                <Input
                    type="number"
                    placeholder="0.00"
                    value={wisata.wisataAmount}
                    onChange={handleInputChange("wisataAmount")}
                />
            </Field>

            <Field label="Percentage" required style={{ marginBottom: "1.5rem" }}>
                <Input
                    type="number"
                    placeholder="0.00"
                    value={wisata.wisataPercentage}
                    onChange={handleInputChange("wisataPercentage")}
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

            {showSuccess && (
                <MessageBar style={{ marginTop: "1rem" }}>
                    Wisata updated successfully.
                </MessageBar>
            )}

            <Dialog open={dialogOpen} onOpenChange={(_, data) => setDialogOpen(data.open)}>
                <DialogSurface>
                    <DialogTitle>Error Saving Wisata</DialogTitle>
                    <DialogBody>
                        <DialogContent>
                            {error || "Unable to update the wisata."}
                        </DialogContent>
                    </DialogBody>
                    <DialogActions>
                        <Button onClick={() => setDialogOpen(false)}>Close</Button>
                    </DialogActions>
                </DialogSurface>
            </Dialog>
        </form>
    );
}

export default EditWisata;

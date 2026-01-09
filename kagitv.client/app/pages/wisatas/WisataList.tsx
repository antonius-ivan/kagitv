import React, { useEffect, useState, type JSX } from "react";
import {
    DataGrid,
    DataGridBody,
    DataGridRow,
    DataGridHeader,
    DataGridHeaderCell,
    DataGridCell,
    TableCellLayout,
    createTableColumn,
    Button,
    Dialog,
    DialogTrigger,
    DialogTitle,
    DialogActions,
    DialogBody,
    DialogSurface,
} from "@fluentui/react-components";
import type { TableColumnDefinition } from "@fluentui/react-components";
import { EditRegular, DeleteRegular } from "@fluentui/react-icons";
import { useNavigate } from "react-router-dom";
import type { IWisata } from "./IWisata";

export function WisataList(): JSX.Element {
    //Hook and all the http fetch
    const [wisatas, setWisatas] = useState<IWisata[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedWisata, setSelectedWisata] = useState<IWisata | null>(null);

    const navigate = useNavigate();

    const fetchWisatas = async () => {
        setLoading(true);
        try {
            const response = await fetch("api/v1/traveloka/wisatas");
            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }
            const data: IWisata[] = await response.json();
            setWisatas(data);
        } catch (err: any) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    //Fetch Use Effect for Render
    useEffect(() => {
        fetchWisatas();
    }, []);


    const handleEdit = (wisata: IWisata) => {
        navigate(`/editwisata/${wisata.wisataid}`);
    };

    const handleDelete = (wisata: IWisata) => {
        setSelectedWisata(wisata);
        setDeleteDialogOpen(true);
    };

    const confirmDelete = async () => {
        if (selectedWisata) {
            const response = await fetch(
                `api/v1/tourney/wisatas/${selectedWisata.wisataid}`,
                { method: "DELETE" }
            );
            if (response.ok) {
                await fetchWisatas();
            }
            setDeleteDialogOpen(false);
            setSelectedWisata(null);
        }
    };

    const columns: TableColumnDefinition<IWisata>[] = [
        createTableColumn<IWisata>({
            columnId: "Nama",
            compare: (a, b) => a.Nama.localeCompare(b.Nama),
            renderHeaderCell: () => "Name",
            renderCell: item => <TableCellLayout>{item.Nama}</TableCellLayout>,
        }),
        createTableColumn<IWisata>({
            columnId: "Kota",
            compare: (a, b) => a.Kota.localeCompare(b.Kota),
            renderHeaderCell: () => "Name",
            renderCell: item => <TableCellLayout>{item.Kota}</TableCellLayout>,
        }),
        createTableColumn<IWisata>({
            columnId: "Harga",
            compare: (a, b) => a.Harga - b.Harga,
            renderHeaderCell: () => "Amount",
            renderCell: item => <TableCellLayout>{item.Harga}</TableCellLayout>,
        }),
        createTableColumn<IWisata>({
            columnId: "actions",
            renderHeaderCell: () => "Actions",
            renderCell: item => (
                <TableCellLayout style={{ display: "flex", gap: "8px" }}>
                    <Button
                        appearance="primary"
                        icon={<EditRegular />}
                        onClick={() => handleEdit(item)}
                        title="Edit"
                        aria-label="Edit Wisata"
                    />
                    <Button
                        icon={<DeleteRegular />}
                        onClick={() => handleDelete(item)}
                        title="Delete"
                        aria-label="Delete Wisata"
                    />
                </TableCellLayout>
            ),
        }),
    ];

    if (loading) return <div>Loading…</div>;
    if (error) return <div style={{ color: "red" }}>Error: {error}</div>;

    return (
            <div>
                <div style={{ marginBottom: "16px" }}>
                    <Button appearance="primary" onClick={() => navigate("/newwisata")}>
                        New Wisata
                    </Button>
                </div>

                <DataGrid
                    items={wisatas}
                    columns={columns}
                    sortable
                    selectionMode="multiselect"
                    getRowId={item => item.id?.toString() ?? ''}
                    focusMode="composite"
                    style={{ minWidth: "650px" }}
                >
                    <DataGridHeader>
                        <DataGridRow
                            selectionCell={{ checkboxIndicator: { "aria-label": "Select all rows" } }}
                        >
                            {({ renderHeaderCell }) => (
                                <DataGridHeaderCell>{renderHeaderCell()}</DataGridHeaderCell>
                            )}
                        </DataGridRow>
                    </DataGridHeader>

                    <DataGridBody<IWisata>>
                        {({ item, rowId }) => (
                        <DataGridRow<IWisata>
                                key={rowId}
                                selectionCell={{ checkboxIndicator: { "aria-label": "Select row" } }}
                            >
                                {({ renderCell }) => <DataGridCell>{renderCell(item)}</DataGridCell>}
                            </DataGridRow>
                        )}
                    </DataGridBody>
                </DataGrid>

                <Dialog open={deleteDialogOpen} onOpenChange={(e, data) => setDeleteDialogOpen(data.open)}>
                    <DialogSurface>
                        <DialogTitle>Are you sure?</DialogTitle>
                        <DialogBody>
                            Do you want to delete the wisata <strong>{selectedWisata?.Nama}</strong>?
                        </DialogBody>
                        <DialogActions>
                            <Button appearance="primary" onClick={confirmDelete}>
                                Delete
                            </Button>
                            <DialogTrigger disableButtonEnhancement>
                                <Button>Close</Button>
                            </DialogTrigger>
                        </DialogActions>
                    </DialogSurface>
                </Dialog>
            </div>
    );
}

export default WisataList;

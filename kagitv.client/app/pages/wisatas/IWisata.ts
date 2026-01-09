export interface IWisata {
    wisataid?: number;           // Optional field for wisata ID
    Kota: string;   // Required field for Wisata Number
    Nama: string;     // Required field for Wisata Name
    Harga: number;   // Required field for Wisata Amount
    createdDate?: Date;    // Optional field for creation date
}

export interface IWisata {
    id?: number;           // Optional field for wisata ID
    wisataNumber: number;   // Required field for Wisata Number
    wisataName: string;     // Required field for Wisata Name
    wisataAmount: number;   // Required field for Wisata Amount
    wisataPercentage: number; // Required field for Wisata Percentage
    createdDate?: Date;    // Optional field for creation date
    createdBy?: string;    // Optional field for creator's name
    updatedDate?: Date;    // Optional field for last update date
    updatedBy?: string;    // Optional field for updater's name
}

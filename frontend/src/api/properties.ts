export type PropertyType = "apartment" | "house" | "condo" | "commercial";
export type PropertyStatus = "active" | "renovation" | "listed";

export interface Property {
  id: string;
  name: string;
  address: string;
  city: string;
  type: PropertyType;
  units: number;
  occupiedUnits: number;
  monthlyRent: number;
  yearBuilt: number;
  status: PropertyStatus;
  gradient: string;
}

export async function getProperties(): Promise<Property[]> {
    const response = await fetch("https://localhost:7119/api/properties");

    if(!response.ok) {
        throw new Error("Failed to fetch properties");
    }

    return response.json();
}
import type { Property, Properties } from "@/types";

export async function getProperties(): Promise<Properties[]> {
    const response = await fetch("https://localhost:7119/api/properties");

    if(!response.ok) {
        throw new Error("Failed to fetch properties");
    }

    return response.json();
}

export async function getProperty(propertyId: string): Promise<Property> {
    const response = await fetch(`https://localhost:7119/api/properties/${propertyId}`)

    if(!response.ok) {
        throw new Error("Failed to fetch properties");
    }

    return response.json();
}
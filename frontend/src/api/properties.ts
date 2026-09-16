import type { Property, Properties, PropertyInput } from "@/types";

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

export async function createProperty(property: PropertyInput): Promise<Property> {
    const response = await fetch(`https://localhost:7119/api/properties`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(property),
    });

    if (!response.ok){
        throw new Error("Failed to create property");
    }

    return response.json();
}
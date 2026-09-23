import type { Booking, BookingFormInput } from "@/types";
import { authFetch } from "@/lib/auth/authFetch";
import { API_URL } from "@/api/config";

export async function getBookings(): Promise<Booking[]> {
    const response = await authFetch(`${API_URL}/api/bookings`);

    if (!response.ok) {
        throw new Error("Failed to fetch bookings");
    }

    return response.json();
}

export async function getBooking(bookingId: string): Promise<Booking> {
    const response = await authFetch(`${API_URL}/api/bookings/${bookingId}`);

    if (!response.ok) {
        throw new Error("Failed to fetch booking");
    }

    return response.json();
}

export async function createBooking(input: BookingFormInput): Promise<Booking> {
    const response = await authFetch(`${API_URL}/api/bookings`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(input),
    });

    if (!response.ok) {
        if (response.status === 409) {
            throw new Error("This rental option is already booked for the selected dates.");
        }
        throw new Error("Failed to create booking");
    }

    return response.json();
}

export async function updateBooking(input: BookingFormInput, bookingId: string): Promise<Booking> {
    const response = await authFetch(`${API_URL}/api/bookings/${bookingId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(input),
    });

    if (!response.ok) {
        if (response.status === 409) {
            throw new Error("This rental option is already booked for the selected dates.");
        }
        throw new Error("Failed to update booking");
    }

    return response.json();
}

export async function deleteBooking(bookingId: string): Promise<void> {
    const response = await authFetch(`${API_URL}/api/bookings/${bookingId}`, {
        method: "DELETE",
    });

    if (!response.ok) {
        throw new Error("Failed to delete booking");
    }
}

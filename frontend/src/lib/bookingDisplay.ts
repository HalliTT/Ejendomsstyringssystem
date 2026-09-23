import type { BadgeVariant } from "@/components/ui/Badge";
import type { BookingStatus } from "@/types";

export const bookingStatusVariant: Record<BookingStatus, BadgeVariant> = {
  Upcoming: "accent",
  Active: "success",
  Completed: "neutral",
};

export function formatBookingDate(value: string): string {
  return new Date(value).toLocaleDateString();
}

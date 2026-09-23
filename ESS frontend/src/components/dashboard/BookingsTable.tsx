import "@/components/dashboard/BookingsTable.css";
import type { Booking } from "@/types";
import { Badge } from "../ui/Badge";
import { Link } from "react-router";
import { bookingStatusVariant, formatBookingDate } from "@/lib/bookingDisplay";

interface BookingsTableProps {
  bookings: Booking[];
}

export function BookingsTable({ bookings }: BookingsTableProps) {
  if (bookings.length === 0) {
    return (
      <p className="bookings-empty">
        No bookings yet. Use "Create booking" to get started.
      </p>
    );
  }

  return (
    <div className="bookings-table">
      <div className="bookings-header-row">
        <span>Tenant</span>
        <span>Property</span>
        <span>Rental option</span>
        <span>Dates</span>
        <span>Status</span>
      </div>

      {bookings.map((booking) => (
        <Link
          key={booking.id}
          to={`/dashboard/bookings/${booking.id}`}
          className="bookings-row"
        >
          <div>
            <div className="bookings-cell-label">Tenant</div>
            <div className="bookings-label">{booking.tenantName}</div>
          </div>
          <div>
            <div className="bookings-cell-label">Property</div>
            <div className="bookings-cell-primary">{booking.propertyName}</div>
          </div>
          <div>
            <div className="bookings-cell-label">Rental option</div>
            <div className="bookings-cell-primary">
              {booking.rentalOptionName}
            </div>
          </div>
          <div>
            <div className="bookings-cell-label">Dates</div>
            <div className="bookings-cell-secondary">
              {formatBookingDate(booking.startDate)} –{" "}
              {formatBookingDate(booking.endDate)}
            </div>
          </div>
          <div>
            <div className="bookings-cell-label">Status</div>
            <Badge variant={bookingStatusVariant[booking.status]}>
              {booking.status}
            </Badge>
          </div>
        </Link>
      ))}
    </div>
  );
}

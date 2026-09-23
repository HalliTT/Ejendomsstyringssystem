import "@/pages/PropertyPage.css";
import "@/sections/OverviewSection.css";
import { useQuery } from "@tanstack/react-query";
import { Link } from "react-router";
import { getProperties } from "@/api/properties";
import { getBookings } from "@/api/bookings";
import { getMyRentalOptions } from "@/api/rentalOptions";
import { Card, CardHeader } from "@/components/ui/Card";
import { Badge } from "@/components/ui/Badge";
import { bookingStatusVariant, formatBookingDate } from "@/lib/bookingDisplay";

export function OverviewSection() {
  const { data: properties } = useQuery({
    queryKey: ["properties"],
    queryFn: getProperties,
  });

  const { data: bookings } = useQuery({
    queryKey: ["bookings"],
    queryFn: getBookings,
  });

  const { data: rentalOptions } = useQuery({
    queryKey: ["rentalOptions"],
    queryFn: getMyRentalOptions,
  });

  const totalProperties = properties?.length ?? 0;
  const totalUnits =
    properties?.reduce((sum, p) => sum + p.totalUnits, 0) ?? 0;
  const occupiedUnits =
    properties?.reduce((sum, p) => sum + p.occupiedUnits, 0) ?? 0;
  const occupancyRate =
    totalUnits > 0 ? Math.round((occupiedUnits / totalUnits) * 100) : 0;

  const activeBookings =
    bookings?.filter((b) => b.status === "Active").length ?? 0;
  const upcomingBookings =
    bookings?.filter((b) => b.status === "Upcoming").length ?? 0;
  const totalBookings = bookings?.length ?? 0;

  const totalMonthlyRent =
    rentalOptions?.reduce((sum, r) => sum + r.monthlyRent, 0) ?? 0;

  const nextBooking = bookings
    ?.filter((b) => b.status === "Upcoming")
    .sort(
      (a, b) => new Date(a.startDate).getTime() - new Date(b.startDate).getTime(),
    )[0];

  return (
    <div className="overview-section">
      <div className="detail-stats-grid overview-stats-grid">
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{totalProperties}</div>
          <div className="detail-stat-label">Properties</div>
        </Card>
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">
            {occupiedUnits}/{totalUnits}
          </div>
          <div className="detail-stat-label">Units occupied</div>
        </Card>
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{occupancyRate}%</div>
          <div className="detail-stat-label">Occupancy rate</div>
        </Card>
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{totalBookings}</div>
          <div className="detail-stat-label">Bookings</div>
        </Card>
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{activeBookings}</div>
          <div className="detail-stat-label">Active now</div>
        </Card>
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{upcomingBookings}</div>
          <div className="detail-stat-label">Upcoming</div>
        </Card>
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{totalMonthlyRent}</div>
          <div className="detail-stat-label">Monthly rent, listed</div>
        </Card>
      </div>

      <div className="detail-section">
        <Card className="card-padded">
          <CardHeader title="Next booking" />
          {!nextBooking ? (
            <p className="overview-empty">No upcoming bookings.</p>
          ) : (
            <Link
              to={`/dashboard/bookings/${nextBooking.id}`}
              className="overview-next-booking"
            >
              <div>
                <div className="overview-next-booking-who">
                  {nextBooking.tenantName}
                </div>
                <div className="overview-next-booking-what">
                  {nextBooking.propertyName} — {nextBooking.rentalOptionName}
                </div>
                <div className="overview-next-booking-when">
                  {formatBookingDate(nextBooking.startDate)} –{" "}
                  {formatBookingDate(nextBooking.endDate)}
                </div>
              </div>
              <Badge variant={bookingStatusVariant[nextBooking.status]}>
                {nextBooking.status}
              </Badge>
            </Link>
          )}
        </Card>
      </div>
    </div>
  );
}

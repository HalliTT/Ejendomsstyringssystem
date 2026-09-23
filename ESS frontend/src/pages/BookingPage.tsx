import "@/pages/PropertyPage.css";
import "@/pages/BookingPage.css";
import { useParams, useNavigate } from "react-router";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getBooking, updateBooking, deleteBooking } from "@/api/bookings";
import { getMyRentalOptions } from "@/api/rentalOptions";
import { getTenants } from "@/api/tenants";
import { ArrowLeftIcon, PencilIcon, TrashIcon } from "@/components/ui/Icons";
import { Card } from "@/components/ui/Card";
import { Badge } from "@/components/ui/Badge";
import { ConfirmDialog } from "@/components/ui/ConfirmDialog";
import { BookingModal } from "@/components/dashboard/BookingModal";
import { useModalState } from "@/hooks/useModalState";
import { useState } from "react";
import type { BookingFormInput } from "@/types";
import { bookingStatusVariant, formatBookingDate } from "@/lib/bookingDisplay";

export function BookingPage() {
  const { bookingId } = useParams();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const editModal = useModalState(false);
  const [isConfirmOpen, setConfirmOpen] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const {
    data: booking,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["booking", bookingId],
    queryFn: () => getBooking(bookingId!),
    enabled: !!bookingId,
  });

  const { data: rentalOptions } = useQuery({
    queryKey: ["rentalOptions"],
    queryFn: getMyRentalOptions,
  });

  const { data: tenants } = useQuery({
    queryKey: ["tenants"],
    queryFn: getTenants,
  });

  const updateBookingMutation = useMutation({
    mutationFn: (data: BookingFormInput) => updateBooking(data, bookingId!),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["booking", bookingId] });
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
    },
    onError: (err: Error) => setErrorMessage(err.message),
  });

  const deleteBookingMutation = useMutation({
    mutationFn: deleteBooking,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
      navigate("/dashboard/bookings");
    },
  });

  const handleConfirmDelete = () => {
    if (!bookingId) return;
    deleteBookingMutation.mutate(bookingId);
  };

  const openEdit = () => {
    setErrorMessage(null);
    editModal.open();
  };

  const submit = async (input: BookingFormInput) => {
    setErrorMessage(null);
    try {
      await updateBookingMutation.mutateAsync(input);
    } catch {
      // surfaced via errorMessage state (set in onError above)
    }
  };

  if (isLoading) {
    return <p>Loading booking...</p>;
  }

  if (error || !booking) {
    return <p>Error loading booking: {error?.message ?? "Not found"}</p>;
  }

  return (
    <div>
      <button
        type="button"
        className="detail-back-btn"
        onClick={() => navigate("/dashboard/bookings")}
      >
        <ArrowLeftIcon width={16} height={16} />
        Back to bookings
      </button>

      <div
        className="detail-hero"
        style={{
          background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
        }}
      >
        <div className="detail-hero-overlay" />
        <div className="detail-hero-content">
          <div>
            <div className="detail-hero-name">{booking.tenantName}</div>
            <div className="detail-hero-address">
              {booking.propertyName} — {booking.rentalOptionName}
            </div>
          </div>

          <div className="detail-hero-meta">
            <Badge variant={bookingStatusVariant[booking.status]}>
              {booking.status}
            </Badge>
            <button type="button" className="detail-hero-btn" onClick={openEdit}>
              <PencilIcon width={15} height={15} />
              Edit
            </button>
            <button
              type="button"
              className="detail-hero-btn detail-hero-btn-danger"
              onClick={() => setConfirmOpen(true)}
            >
              <TrashIcon width={15} height={15} />
              Delete
            </button>
          </div>
        </div>
      </div>

      <div className="detail-section">
        <Card className="card-padded">
          <div className="booking-detail-grid">
            <div>
              <div className="booking-detail-label">Tenant</div>
              <div className="booking-detail-value">{booking.tenantName}</div>
              <div className="booking-detail-secondary">
                {booking.tenantEmail}
              </div>
              <div className="booking-detail-secondary">
                {booking.tenantPhone}
              </div>
            </div>
            <div>
              <div className="booking-detail-label">Property</div>
              <div className="booking-detail-value">{booking.propertyName}</div>
            </div>
            <div>
              <div className="booking-detail-label">Rental option</div>
              <div className="booking-detail-value">
                {booking.rentalOptionName}
              </div>
            </div>
            <div>
              <div className="booking-detail-label">Dates</div>
              <div className="booking-detail-value">
                {formatBookingDate(booking.startDate)} –{" "}
                {formatBookingDate(booking.endDate)}
              </div>
            </div>
          </div>
        </Card>
      </div>

      <BookingModal
        isOpen={editModal.isOpen}
        onClose={() => {
          editModal.close();
          updateBookingMutation.reset();
          setErrorMessage(null);
        }}
        onSubmit={submit}
        rentalOptions={rentalOptions ?? []}
        tenants={tenants ?? []}
        initialValues={{
          rentalOptionId: booking.rentalOptionId,
          tenantId: booking.tenantId,
          startDate: booking.startDate.slice(0, 10),
          endDate: booking.endDate.slice(0, 10),
        }}
        errorMessage={errorMessage}
        isSubmitting={updateBookingMutation.isPending}
        isSuccess={updateBookingMutation.isSuccess}
      />
      <ConfirmDialog
        isOpen={isConfirmOpen}
        title="Delete booking"
        message={`Are you sure you want to delete this booking for ${booking.tenantName}?`}
        confirmLabel="Delete booking"
        onConfirm={handleConfirmDelete}
        onCancel={() => setConfirmOpen(false)}
      />
    </div>
  );
}

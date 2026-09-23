import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getBookings, createBooking } from "@/api/bookings";
import { getMyRentalOptions } from "@/api/rentalOptions";
import { getTenants } from "@/api/tenants";
import { BookingsTable } from "@/components/dashboard/BookingsTable";
import { BookingModal } from "@/components/dashboard/BookingModal";
import { Card, CardHeader } from "@/components/ui/Card";
import { PlusIcon } from "@/components/ui/Icons";
import { useModalState } from "@/hooks/useModalState";
import { useState } from "react";
import type { BookingFormInput } from "@/types";

export function BookingsSection() {
  const queryClient = useQueryClient();
  const bookingModal = useModalState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const {
    data: bookings,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["bookings"],
    queryFn: getBookings,
  });

  const { data: rentalOptions } = useQuery({
    queryKey: ["rentalOptions"],
    queryFn: getMyRentalOptions,
  });

  const { data: tenants } = useQuery({
    queryKey: ["tenants"],
    queryFn: getTenants,
  });

  const createBookingMutation = useMutation({
    mutationFn: createBooking,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
    },
    onError: (err: Error) => {
      setErrorMessage(err.message);
    },
  });

  const openCreateBooking = () => {
    setErrorMessage(null);
    createBookingMutation.reset();
    bookingModal.open();
  };

  const submit = async (input: BookingFormInput) => {
    setErrorMessage(null);
    try {
      await createBookingMutation.mutateAsync(input);
    } catch {
    }
  };

  if (isLoading) {
    return <p>Loading bookings...</p>;
  }

  if (error) {
    return <p>Error loading bookings: {error.message}</p>;
  }

  return (
    <div>
      <Card className="card-padded">
        <CardHeader
          title="Bookings"
          subtitle={`${bookings?.length ?? 0} total`}
          action={
            <button
              type="button"
              className="detail-section-add-btn"
              onClick={openCreateBooking}
            >
              <PlusIcon width={15} height={15} />
              Create booking
            </button>
          }
        />
        <BookingsTable bookings={bookings ?? []} />
      </Card>

      <BookingModal
        isOpen={bookingModal.isOpen}
        onClose={() => {
          bookingModal.close();
          createBookingMutation.reset();
          setErrorMessage(null);
        }}
        onSubmit={submit}
        rentalOptions={rentalOptions ?? []}
        tenants={tenants ?? []}
        errorMessage={errorMessage}
        isSubmitting={createBookingMutation.isPending}
        isSuccess={createBookingMutation.isSuccess}
      />
    </div>
  );
}

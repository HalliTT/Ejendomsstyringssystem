import "@/components/dashboard/PropertyModal.css";
import "@/components/dashboard/BookingModal.css";
import type { BookingFormInput, RentalOptionSummary, Tenant } from "@/types";
import { useEffect, useState } from "react";
import { Modal } from "../ui/Modal";
import { CheckCircleIcon, LoaderIcon } from "../ui/Icons";

interface BookingModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (input: BookingFormInput) => Promise<void>;
  rentalOptions: RentalOptionSummary[];
  tenants: Tenant[];
  initialValues?: BookingFormInput;
  errorMessage?: string | null;
  isSubmitting?: boolean;
  isSuccess?: boolean;
}

const BlankForm: BookingFormInput = {
  rentalOptionId: "",
  tenantId: "",
  startDate: "",
  endDate: "",
};

export function BookingModal({
  isOpen,
  onClose,
  onSubmit,
  rentalOptions,
  tenants,
  initialValues,
  errorMessage,
  isSubmitting,
  isSuccess,
}: BookingModalProps) {
  const [data, setData] = useState<BookingFormInput>(initialValues ?? BlankForm);
  const isEdit = !!initialValues;

  useEffect(() => {
    if (isOpen) setData(initialValues ?? BlankForm);
  }, [isOpen, initialValues]);

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (
      !data.rentalOptionId ||
      !data.tenantId ||
      !data.startDate ||
      !data.endDate
    ) {
      return;
    }

    await onSubmit(data);
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={isEdit ? "Edit booking" : "Create a booking"}
    >
      <form className="property-modal" onSubmit={handleSubmit}>
        <div className="property-modal-field">
          <label htmlFor="booking-rental-option">Rental option</label>
          <select
            id="booking-rental-option"
            required
            value={data.rentalOptionId}
            onChange={(e) =>
              setData({ ...data, rentalOptionId: e.target.value })
            }
            disabled={isSubmitting || isSuccess}
          >
            <option value="" disabled>
              Select a rental option
            </option>
            {rentalOptions.map((option) => (
              <option key={option.id} value={option.id}>
                {option.propertyName} — {option.name}
              </option>
            ))}
          </select>
        </div>

        <div className="property-modal-field">
          <label htmlFor="booking-tenant">Tenant</label>
          <select
            id="booking-tenant"
            required
            value={data.tenantId}
            onChange={(e) => setData({ ...data, tenantId: e.target.value })}
            disabled={isSubmitting || isSuccess}
          >
            <option value="" disabled>
              Select a tenant
            </option>
            {tenants.map((tenant) => (
              <option key={tenant.id} value={tenant.id}>
                {tenant.name}
              </option>
            ))}
          </select>
        </div>

        <div className="property-modal-row">
          <div className="property-modal-field">
            <label htmlFor="booking-start-date">Start date</label>
            <input
              id="booking-start-date"
              type="date"
              required
              value={data.startDate}
              onChange={(e) =>
                setData({ ...data, startDate: e.target.value })
              }
              disabled={isSubmitting || isSuccess}
            />
          </div>
          <div className="property-modal-field">
            <label htmlFor="booking-end-date">End date</label>
            <input
              id="booking-end-date"
              type="date"
              required
              min={data.startDate || undefined}
              value={data.endDate}
              onChange={(e) => setData({ ...data, endDate: e.target.value })}
              disabled={isSubmitting || isSuccess}
            />
          </div>
        </div>

        {errorMessage && (
          <p className="booking-modal-error">{errorMessage}</p>
        )}

        <div className="property-modal-action">
          <button
            type="button"
            className="property-modal-cancel"
            onClick={onClose}
            disabled={isSubmitting || isSuccess}
          >
            Cancel
          </button>
          <button
            type={isSuccess ? "button" : "submit"}
            className="property-modal-submit"
            style={isSuccess ? { backgroundColor: "var(--color-success)" } : {}}
            onClick={isSuccess ? onClose : undefined}
          >
            {isSubmitting ? (
              <LoaderIcon />
            ) : isSuccess ? (
              <div className="property-modal-succe-wrapper">
                <p>Done</p>
                <CheckCircleIcon />
              </div>
            ) : isEdit ? (
              "Save changes"
            ) : (
              "Create booking"
            )}
          </button>
        </div>
      </form>
    </Modal>
  );
}

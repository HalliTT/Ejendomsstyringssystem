import "@/components/dashboard/PropertyModal.css";
import "@/components/dashboard/RentalOptionModal.css";
import type { RentalOptionFormInput, RentalOptionStatus, Unit } from "@/types";
import { useEffect, useState } from "react";
import { Modal } from "../ui/Modal";
import { CheckCircleIcon, LoaderIcon } from "../ui/Icons";

interface RentalOptionModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (input: RentalOptionFormInput) => Promise<void>;
  units: Unit[];
  initialValues?: RentalOptionFormInput;
  isSubmitting?: boolean;
  isSuccess?: boolean;
}

const BlankForm: RentalOptionFormInput = {
  name: "",
  monthlyRent: 0,
  status: "Available",
  unitIds: [],
};

export function RentalOptionModal({
  isOpen,
  onClose,
  onSubmit,
  units,
  initialValues,
  isSubmitting,
  isSuccess,
}: RentalOptionModalProps) {
  const [data, setData] = useState<RentalOptionFormInput>(initialValues ?? BlankForm);
  const isEdit = !!initialValues;

  useEffect(() => {
    if (isOpen) setData(initialValues ?? BlankForm);
  }, [isOpen, initialValues]);

  const toggleUnit = (unitId: string) => {
    setData((prev) => ({
      ...prev,
      unitIds: prev.unitIds.includes(unitId)
        ? prev.unitIds.filter((id) => id !== unitId)
        : [...prev.unitIds, unitId],
    }));
  };

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!data.name.trim() || data.unitIds.length === 0) {
      return;
    }

    await onSubmit(data);
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={isEdit ? "Edit rental option" : "Add a rental option"}
    >
      <form className="property-modal" onSubmit={handleSubmit}>
        <div className="property-modal-field">
          <label htmlFor="rental-option-name">Name</label>
          <input
            id="rental-option-name"
            required
            value={data.name}
            onChange={(e) => setData({ ...data, name: e.target.value })}
            placeholder="e.g Whole apartment"
            disabled={isSubmitting || isSuccess}
          />
        </div>

        <div className="property-modal-row">
          <div className="property-modal-field">
            <label htmlFor="rental-option-rent">Monthly rent</label>
            <input
              id="rental-option-rent"
              type="number"
              min={0}
              step="0.01"
              required
              value={data.monthlyRent}
              onChange={(e) =>
                setData({ ...data, monthlyRent: Number(e.target.value) })
              }
              disabled={isSubmitting || isSuccess}
            />
          </div>
          <div className="property-modal-field">
            <label htmlFor="rental-option-status">Status</label>
            <select
              id="rental-option-status"
              value={data.status}
              onChange={(e) =>
                setData({
                  ...data,
                  status: e.target.value as RentalOptionStatus,
                })
              }
              disabled={isSubmitting || isSuccess}
            >
              <option value="Available">Available</option>
              <option value="Unavailable">Unavailable</option>
            </select>
          </div>
        </div>

        <div className="property-modal-field">
          <label>Units included</label>
          <div className="rental-option-unit-picker">
            {units.length === 0 && (
              <p className="rental-option-unit-picker-empty">
                No units on this property yet.
              </p>
            )}
            {units.map((unit) => (
              <label key={unit.id} className="rental-option-unit-checkbox">
                <input
                  type="checkbox"
                  checked={data.unitIds.includes(unit.id)}
                  onChange={() => toggleUnit(unit.id)}
                  disabled={isSubmitting || isSuccess}
                />
                {unit.name}
              </label>
            ))}
          </div>
        </div>

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
              "Add rental option"
            )}
          </button>
        </div>
      </form>
    </Modal>
  );
}

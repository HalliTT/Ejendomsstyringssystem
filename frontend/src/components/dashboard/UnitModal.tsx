import "@/components/dashboard/PropertyModal.css";
import "@/components/dashboard/UnitModal.css";
import { UnitStatus, type UnitInput } from "@/types";
import { useEffect, useState } from "react";
import { Modal } from "../ui/Modal";
import { CheckCircleIcon, LoaderIcon } from "../ui/Icons";

interface UnitModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (input: UnitInput) => Promise<void>;
  initialValues?: UnitInput;
  isSubmitting?: boolean;
  isSuccess?: boolean;
}

const BlankForm: UnitInput = {
  name: "",
  description: "",
  status: UnitStatus.Available,
};

export function UnitModal({
  isOpen,
  onClose,
  onSubmit,
  initialValues,
  isSubmitting,
  isSuccess,
}: UnitModalProps) {
  const [data, setData] = useState<UnitInput>(initialValues ?? BlankForm);
  const isEdit = !!initialValues;

  useEffect(() => {
    if (isOpen) setData(initialValues ?? BlankForm);
  }, [isOpen, initialValues]);

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!data.name.trim() || !data.description.trim()) {
      return;
    }

    await onSubmit(data);
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={isEdit ? "Edit unit" : "Add a unit"}
    >
      <form className="property-modal" onSubmit={handleSubmit}>
        <div className="property-modal-field">
          <label htmlFor="unit-name">Unit name</label>
          <input
            id="unit-name"
            required
            value={data.name}
            onChange={(e) => setData({ ...data, name: e.target.value })}
            placeholder="e.g First Floor"
            disabled={isSubmitting || isSuccess}
          />
        </div>

        <div className="property-modal-field">
          <label htmlFor="unit-description">Description</label>
          <input
            id="unit-description"
            required
            value={data.description}
            onChange={(e) => setData({ ...data, description: e.target.value })}
            placeholder="e.g This unit is..."
            disabled={isSubmitting || isSuccess}
          />
        </div>

        <div className="property-modal-row">
          <div className="property-modal-field">
            <label htmlFor="unit-status">Status</label>
            <select
              id="unit-status"
              value={data.status}
              onChange={(e) =>
                setData({ ...data, status: e.target.value as UnitStatus })
              }
              disabled={isSubmitting || isSuccess}
            >
              <option value={UnitStatus.Available}>Available</option>
              <option value={UnitStatus.Occupied}>Occupied</option>
              <option value={UnitStatus.Maintenance}>Under maintenance</option>
            </select>
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
              "Add unit"
            )}
          </button>
        </div>
      </form>
    </Modal>
  );
}

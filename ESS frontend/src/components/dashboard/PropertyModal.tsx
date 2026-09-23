import "@/components/dashboard/PropertyModal.css";
import { type PropertyInput } from "@/types";
import { useEffect, useState, type FormEvent } from "react";
import { Modal } from "../ui/Modal";
import { CheckCircleIcon, LoaderIcon } from "../ui/Icons";

interface PropertyModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (input: PropertyInput) => Promise<void>;
  initialValues?: PropertyInput;
  isSubmitting?: boolean;
  isSuccess?: boolean;
}

const BlankForm: PropertyInput = {
  name: "",
  address: "",
  city: "",
  country: "",
  description: "",
};

export function PropertyModal({
  isOpen,
  onClose,
  onSubmit,
  initialValues,
  isSubmitting,
  isSuccess,
}: PropertyModalProps) {
  const [data, setData] = useState<PropertyInput>(initialValues ?? BlankForm);
  const isEdit = !!initialValues;

  useEffect(() => {
    if (isOpen) setData(initialValues ?? BlankForm);
  }, [isOpen, initialValues]);

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (
      !data.name.trim() ||
      !data.address.trim() ||
      !data.city.trim() ||
      !data.country.trim() ||
      !data.description.trim()
    ) {
      return;
    }

    await onSubmit(data);
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={isEdit ? "Edit property" : "Add a property"}
    >
      <form className="property-modal" onSubmit={handleSubmit}>
        <div className="property-modal-field">
          <label htmlFor="property-name">Property name</label>
          <input
            id="property-name"
            required
            value={data.name}
            onChange={(e) => setData({ ...data, name: e.target.value })}
            placeholder="e.g Fjordveien Residence"
            disabled={isSubmitting || isSuccess}
          />
        </div>

        <div className="property-modal-field">
          <label htmlFor="property-address">Street address</label>
          <input
            id="property-address"
            required
            value={data.address}
            onChange={(e) => setData({ ...data, address: e.target.value })}
            placeholder="e.g Fjordveien 12"
            disabled={isSubmitting || isSuccess}
          />
        </div>

        <div className="property-modal-row">
          <div className="property-modal-field">
            <label htmlFor="property-city">City</label>
            <input
              id="property-city"
              required
              value={data.city}
              onChange={(e) => setData({ ...data, city: e.target.value })}
              placeholder="e.g Oslo"
              disabled={isSubmitting || isSuccess}
            />
          </div>
          <div className="property-modal-field">
            <label htmlFor="property-country">Country</label>
            <input
              id="property-country"
              required
              value={data.country}
              onChange={(e) => setData({ ...data, country: e.target.value })}
              placeholder="e.g Norway"
              disabled={isSubmitting || isSuccess}
            />
          </div>
        </div>

        <div className="property-modal-field">
          <label htmlFor="property-description">Description</label>
          <input
            id="property-description"
            required
            value={data.description}
            onChange={(e) => setData({ ...data, description: e.target.value })}
            placeholder="e.g This house is outs..."
            disabled={isSubmitting || isSuccess}
          />
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
              "Add property"
            )}
          </button>
        </div>
      </form>
    </Modal>
  );
}

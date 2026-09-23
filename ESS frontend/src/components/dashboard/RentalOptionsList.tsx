import "@/components/dashboard/RentalOptionsList.css";
import type { RentalOption, Unit } from "@/types";
import { Badge } from "../ui/Badge";
import { PencilIcon, TrashIcon } from "../ui/Icons";

interface RentalOptionsListProps {
  rentalOptions: RentalOption[];
  units?: Unit[];
  onEdit: (option: RentalOption) => void;
  onDelete: (option: RentalOption) => void;
}

export function RentalOptionsList({
  rentalOptions,
  units,
  onEdit,
  onDelete,
}: RentalOptionsListProps) {
  if (rentalOptions.length === 0) {
    return (
      <p className="rental-options-empty">
        No rental options yet. Add one to to define what you offer.
      </p>
    );
  }

  return (
    <div className="rental-options-list">
      {rentalOptions.map((option) => {
        const optionUnits = units?.filter((unit) =>
          option.unitIds.includes(unit.id),
        );

        return (
          <div key={option.id} className="rental-options-row">
            <div className="rental-options-main">
              <div className="rental-options-row-title">
                <span className="rental-option-name">{option.name}</span>
                <Badge variant="success">{option.status}</Badge>
              </div>

              <div className="rental-options-rent">
                {option.monthlyRent} / month
              </div>

              <div className="rental-options-chips">
                {optionUnits?.map((unit) => (
                  <Badge key={unit.id} variant="neutral">
                    {unit.name}
                  </Badge>
                ))}
              </div>
            </div>

            <div className="rental-options-side">
              <div className="rental-options-actions">
                <button
                  type="button"
                  className="rental-options-action-btn"
                  onClick={() => onEdit(option)}
                  aria-label={`Edit rental options ${option.name}`}
                >
                  <PencilIcon width={15} height={15} />
                </button>
                <button
                  type="button"
                  className="rental-options-action-btn rental-options-action-btn-danger"
                  onClick={() => onDelete(option)}
                  aria-label={`Delete rental option ${option.name}`}
                >
                  <TrashIcon width={15} height={15} />
                </button>
              </div>
            </div>
          </div>
        );
      })}
    </div>
  );
}

import "@/components/dashboard/UnitsTable.css";
import type { Unit } from "@/types";
import { Badge } from "../ui/Badge";
import { PencilIcon, TrashIcon } from "../ui/Icons";

interface UnitsTableProps {
  units?: Unit[];
  onEdit: (unit: Unit) => void;
  onDelete: (unit: Unit) => void;
}

export function UnitsTable({ units, onEdit, onDelete }: UnitsTableProps) {
  if (units?.length === 0) {
    return (
      <p className="units-empty">
        No units added. Use "Add unit" to start building.
      </p>
    );
  }

  return (
    <div className="units-table">
      <div className="units-header-row">
        <span>Unit</span>
        <span>Description</span>
        <span>Status</span>
        <span />
      </div>

      {units?.map((unit) => {
        return (
          <div key={unit.id} className="units-row">
            <div>
              <div className="units-cell-label">Unit</div>
              <div className="units-name units-label">{unit.name}</div>
            </div>

            <div>
              <div className="units-cell-label">Description</div>
              <div className="units-cell-primary">{unit.description}</div>
            </div>

            <div>
              <div className="units-cell-label">Status</div>
              <div>
                <Badge variant="purple">{unit.status}</Badge>
              </div>
            </div>

            <div>
              <div className="units-actions">
                <button
                  type="button"
                  className="units-action-btn"
                  onClick={() => {}}
                  aria-label={`Edit unit ${unit.name}`}
                >
                  <PencilIcon width={15} height={15} />
                </button>
                <button
                  type="button"
                  className="units-action-btn units-action-btn-danger"
                  onClick={() => {}}
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

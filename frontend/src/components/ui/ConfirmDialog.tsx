import { Modal } from "./Modal";
import "@/components/ui/ConfirmDialog.css";

const styles = {
  actions: "fc-actions",
  cancelButton: "fc-cancel-btn",
  dangerButton: "fc-danger-btn",
};

interface ConfirmDialogProps {
  isOpen: boolean;
  title: string;
  message: string;
  confirmLabel?: string;
  onConfirm: () => void;
  onCancel: () => void;
}

export function ConfirmDialog({
  isOpen,
  title,
  message,
  confirmLabel = "Delete",
  onConfirm,
  onCancel,
}: ConfirmDialogProps) {
  return (
    <Modal isOpen={isOpen} onClose={onCancel} title={title}>
      <p
        style={{
          color: "var(--color-text-secondary)",
          fontSize: "0.9rem",
          lineHeight: 1.5,
        }}
      >
        {message}
      </p>
      <div className={styles.actions} style={{ marginTop: "1.25rem" }}>
        <button
          type="button"
          className={styles.cancelButton}
          onClick={onCancel}
        >
          Cancel
        </button>
        <button
          type="button"
          className={styles.dangerButton}
          onClick={onConfirm}
        >
          {confirmLabel}
        </button>
      </div>
    </Modal>
  );
}

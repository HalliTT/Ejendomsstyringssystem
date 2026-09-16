import "@/components/ui/Modal.css";
import { useEffect, type ReactNode } from "react";
import { IconButton } from "./IconButton";
import { CloseIcon } from "./Icons";

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: ReactNode;
}

export function Modal({ isOpen, onClose, title, children }: ModalProps) {
  useEffect(() => {
    if (!isOpen) return;

    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === "Escape") onClose();
    };

    return () => {
      document.addEventListener("keydown", handleKeyDown);
      document.body.style.overflow = "hidden";
    };
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose} role="presentation">
      <div
        className="modal"
        role="dialog"
        aria-modal="true"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="modal-header">
          <h2 id="modal-title" className="modal-title">
            {title}
          </h2>
          <IconButton aria-label="Close dialog" onClick={onClose}>
            <CloseIcon width={18} height={18} />
          </IconButton>
        </div>
        {children}
      </div>
    </div>
  );
}

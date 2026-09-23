import { useEffect, useRef, useState } from "react";
import { ChevronDownIcon, LogoutIcon } from "../ui/Icons";
import { Avatar } from "../ui/Avatar";
import { ConfirmDialog } from "../ui/ConfirmDialog";
import "@/components/layout/ProfileMenu.css";

interface ProfileMenuProps {
  name: string;
  email: string;
  avatar: string;
  isOpen: boolean;
  onToggle: () => void;
  onClose: () => void;
  onLogout: () => void;
}

export function ProfileMenu({
  name,
  email,
  avatar,
  isOpen,
  onToggle,
  onClose,
  onLogout,
}: ProfileMenuProps) {
  const wrapperRef = useRef<HTMLDivElement>(null);
  const [isConfirmOpen, setConfirmOpen] = useState(false);

  useEffect(() => {
    if (!isOpen) return;

    const handleClickOutside = (e: MouseEvent) => {
      if (
        wrapperRef.current &&
        !wrapperRef.current.contains(e.target as Node)
      ) {
        onClose();
      }
    };

    document.addEventListener("mousedown", handleClickOutside);

    return document.removeEventListener("mousedown", handleClickOutside);
  }, [isOpen, onClose]);

  const handleLogoutClick = () => {
    onClose();
    setConfirmOpen(true);
  };

  const handleConfirmLogout = () => {
    setConfirmOpen(false);
    onLogout();
  };

  return (
    <div className="profile-wrapper" ref={wrapperRef}>
      <button
        type="button"
        className="profile-trigger"
        onClick={onToggle}
        aria-label="Account menu"
        aria-haspopup="menu"
        aria-expanded={isOpen}
      >
        <Avatar name={name} avatar={avatar} size={36} />
        <ChevronDownIcon
          width={14}
          height={14}
          className={`"profile-trigger-chevron" ${isOpen ? "profile-trigger-chevron-open" : ""}`}
        ></ChevronDownIcon>
      </button>

      {isOpen && (
        <div className="profile-panel" role="menu">
          <div className="profile-panel-header">
            <Avatar name={name} avatar={avatar} size={40} />
            <div>
              <div className="profile-panel-name">{name}</div>
              <div className="profile-panel-email">{email}</div>
            </div>
          </div>

          <div className="profile-panel-divider" />

          <button
            type="button"
            className="profile-panel-btn"
            role="menuitem"
            onClick={handleLogoutClick}
          >
            <LogoutIcon width={17} height={17} />
            Log out
          </button>
        </div>
      )}

      <ConfirmDialog
        isOpen={isConfirmOpen}
        title="Log out"
        message={`Are you sure you want to log out, ${name.split(" ")[0]}?`}
        confirmLabel="Log out"
        onConfirm={handleConfirmLogout}
        onCancel={() => setConfirmOpen(false)}
      />
    </div>
  );
}

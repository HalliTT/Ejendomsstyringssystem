import "./Avatar.css";

const styles = {
  avatar: "avatar",
};

interface AvatarProps {
  name: string;
  avatar: string;
  size?: number;
}

function getInitials(name: string): string {
  const parts = name.trim().split(/\s+/);
  const first = parts[0]?.[0] ?? "";
  const last = parts.length > 1 ? (parts[parts.length - 1]?.[0] ?? "") : "";
  return (first + last).toUpperCase();
}

export function Avatar({ name, avatar, size = 36 }: AvatarProps) {
  return (
    <span
      className={styles.avatar}
      style={{
        width: size,
        height: size,
        background: avatar,
        fontSize: size * 0.38,
      }}
    >
      {getInitials(name)}
    </span>
  );
}

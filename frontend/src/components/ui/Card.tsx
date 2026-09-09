import "@/components/ui/Card.css";

interface CardProps {
  title: string;
  content: string;
}

export function Card({ title, content }: CardProps) {
  return (
    <div className="card">
      <h2>{title}</h2>
      <p>{content}</p>
    </div>
  );
}

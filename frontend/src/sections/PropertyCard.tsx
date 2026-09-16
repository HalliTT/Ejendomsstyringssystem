import "@/sections/PropertyCard.css";
import type { Properties } from "@/types";
import { Card } from "@/components/ui/Card";
import { MapPinIcon } from "@/components/ui/Icons";
import { Badge } from "@/components/ui/Badge";
import { Link } from "react-router";

interface PropertyCardProps {
  property: Properties;
}

export function PropertyCard({ property }: PropertyCardProps) {
  return (
    <Link
      to={`/dashboard/properties/${property.id}`}
      className="property-card-link"
    >
      <Card className="property-card">
        <div
          className="property-card-cover"
          style={{
            background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
          }}
        >
          <Badge variant="warning">
            {property.occupiedUnits} / {property.totalUnits}
          </Badge>
        </div>
        <div className="property-card-body">
          <div>
            <div className="property-card-name">{property.name}</div>
            <div className="property-card-address">
              <MapPinIcon width={13} height={13} />
              {property.address}
            </div>
            <div className="property-card-status">
              <div className="property-card-status-item">
                <span className="status-dot status-dot-occupied" />
                <span>
                  <strong>{property.occupiedUnits}</strong> occupied
                </span>
              </div>
              <div className="property-card-status-item">
                <span className="status-dot status-dot-available" />
                <span>
                  <strong>
                    {property.totalUnits - property.occupiedUnits}
                  </strong>{" "}
                  available
                </span>
              </div>
            </div>
            <div></div>
          </div>
        </div>
      </Card>
    </Link>
  );
}

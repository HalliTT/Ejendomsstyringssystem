import "@/pages/PropertyPage.css";
import { useParams } from "react-router";
import { useQuery } from "@tanstack/react-query";
import { getProperty } from "@/api/properties";
import type { Property, Unit } from "@/types";

import {
  ArrowLeftIcon,
  MapPinIcon,
  PencilIcon,
  PlusIcon,
  TrashIcon,
} from "@/components/ui/Icons";
import { Card, CardHeader } from "@/components/ui/Card";
import { UnitsTable } from "@/components/dashboard/UnitsTable";

interface PropertyDetailPageProps {
  onBack: () => void;
}

export function PropertyPage({ onBack }: PropertyDetailPageProps) {
  const { propertyId } = useParams();

  const {
    data: property,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["property", propertyId],
    queryFn: () => getProperty(propertyId!),
  });

  if (isLoading) {
    return <p>Loading properties...</p>;
  }

  if (error) {
    return <p>Error loading properties: {error.message}</p>;
  }

  return (
    <div>
      <button type="button" className="detail-back-btn" onClick={onBack}>
        <ArrowLeftIcon width={16} height={16} />
        Back to properties
      </button>

      <div
        className="detail-hero"
        style={{
          background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
        }}
      >
        <div className="detail-hero-overlay" />
        <div className="detail-hero-content">
          <div>
            <div className="detail-hero-name">{property?.name}</div>
            <div className="detail-hero-address">
              <MapPinIcon width={14} height={14} />
              {property?.address}, {property?.city}
            </div>
          </div>

          <div className="detail-hero-meta">
            <button
              type="button"
              className="detail-hero-btn"
              onClick={() => {}}
            >
              <PencilIcon width={15} height={15} />
              Edit
            </button>
            <button
              type="button"
              className="detail-hero-btn detail-hero-btn-danger"
              onClick={() => {}}
            >
              <TrashIcon width={15} height={15} />
              Delete
            </button>
          </div>
        </div>
      </div>

      <div className="detail-stats-grid">
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{property?.totalUnits}</div>
          <div className="detail-stat-label">Total units</div>
        </Card>
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{property?.occupiedUnits}</div>
          <div className="detail-stat-label">Occupied</div>
        </Card>
      </div>

      <div className="detail-section">
        <Card className="card-padded">
          <CardHeader
            title="Units"
            subtitle={`${property?.totalUnits} total · ${property?.occupiedUnits} occupied`}
            action={
              <button
                type="button"
                className="detail-section-add-btn"
                onClick={() => {}}
              >
                <PlusIcon width={15} height={15} />
                Add unit
              </button>
            }
          />
          <UnitsTable
            units={property?.units}
            onEdit={() => {}}
            onDelete={() => {}}
          />
        </Card>
      </div>
    </div>
  );
}

import { useQuery } from "@tanstack/react-query";
import "@/sections/PropertiesSection.css";
import { getProperties } from "@/api/properties";
import { PropertyCard } from "@/sections/PropertyCard";
import { useMemo, useState } from "react";

type OccupancyFilter = "all" | "vacant" | "full";

const Filter_Type: Array<{ id: OccupancyFilter; label: string }> = [
  { id: "all", label: "All properties" },
  { id: "vacant", label: "Has vacancies" },
  { id: "full", label: "Fully occupied" },
];

export function PropertiesSection() {
  const [occupancyFilter, setOccupancyFilter] = useState<OccupancyFilter>("all");

  const {
    data: properties,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["properties"],
    queryFn: getProperties,
  });

  const filteredProperties = useMemo(() => {
    return properties?.filter((property) => {
      if (occupancyFilter === "vacant") {
        return property.occupiedUnits < property.totalUnits;
      }
      if (occupancyFilter === "full") {
        return (
          property.totalUnits > 0 &&
          property.occupiedUnits === property.totalUnits
        );
      }
      return true;
    });
  }, [properties, occupancyFilter]);

  if (isLoading) {
    return <p>Loading properties...</p>;
  }

  if (error) {
    return <p>Error loading properties: {error.message}</p>;
  }

  return (
    <div>
      <div className="properties-toolbar">
        {Filter_Type.map((filter) => (
          <button
            key={filter.id}
            type="button"
            className={`properties-pill ${occupancyFilter === filter.id ? "properties-pill-active" : ""}`}
            onClick={() => setOccupancyFilter(filter.id)}
          >
            {filter.label}
          </button>
        ))}
      </div>

      {filteredProperties?.length === 0 ? (
        <p className="properties-grid-empty">
          {occupancyFilter === "all"
            ? `No properties yet. Use "Add property" to start building.`
            : "No properties match this filter."}
        </p>
      ) : (
        <div className="properties-grid">
          {filteredProperties?.map((property) => (
            <PropertyCard key={property.id} property={property}></PropertyCard>
          ))}
        </div>
      )}
    </div>
  );
}

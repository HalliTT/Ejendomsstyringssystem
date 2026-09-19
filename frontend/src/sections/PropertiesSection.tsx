import { useQuery } from "@tanstack/react-query";
import "@/sections/PropertiesSection.css";
import { getProperties } from "@/api/properties";
import { PropertyCard } from "@/sections/PropertyCard";
import { useMemo, useState } from "react";

type PropertyType = "apartment" | "house";

const Filter_Type: Array<{ id: PropertyType | "all"; label: string }> = [
  { id: "all", label: "All types" },
  { id: "house", label: "House" },
  { id: "apartment", label: "Apartment building" },
];

export function PropertiesSection() {
  const [typeFilter, setTypeFilter] = useState<PropertyType | "all">("all");

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
      const matchedType = typeFilter === "all" || property.name === typeFilter;

      return matchedType;
    });
  }, [properties, typeFilter]);

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
            className={`properties-pill ${typeFilter === filter.id ? "properties-pill-active" : ""}`}
            onClick={() => setTypeFilter(filter.id)}
          >
            {filter.label}
          </button>
        ))}
      </div>

      {filteredProperties?.length === 0 ? (
        <p className="properties-grid-empty">
          {typeFilter === "all"
            ? `No properties yet. Use "Add property" to start building.`
            : "No properties match your search."}
        </p>
      ) : (
        <div className="properties-grid">
          {properties?.map((property) => (
            <PropertyCard key={property.id} property={property}></PropertyCard>
          ))}
        </div>
      )}
    </div>
  );
}

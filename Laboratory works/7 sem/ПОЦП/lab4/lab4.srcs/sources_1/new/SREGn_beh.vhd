----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 01:22:06 PM
-- Design Name: 
-- Module Name: SREGn - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity SREGn is
    Generic (n : integer := 4);
    Port ( Sin : in STD_LOGIC;
           Sout : out STD_LOGIC_VECTOR (0 to n-1);
           SE : in STD_LOGIC;
           Clk : in STD_LOGIC;
           Rst : in STD_LOGIC);
end SREGn;

architecture Behavioral of SREGn is
    signal sreg : std_logic_vector(0 to n-1);
begin
    main: process (Sin, SE, Clk, Rst)
    begin
        if Rst = '1' then
            sreg <= (others => '0');
        elsif rising_edge(Clk) then
            if SE = '1' then
                for i in 0 to n-2 loop
                    sreg(i+1) <= sreg(i);
                end loop;
                sreg(0) <= Sin;
            end if;
        end if;
    end process;
    
    Sout <= sreg;
end Behavioral;
